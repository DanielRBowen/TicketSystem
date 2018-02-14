using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Models;
using TicketSystem.ViewModels;

namespace TicketSystem.Controllers
{
    public class HomeController : Controller
    {
        private TicketContext ApplicationContext { get; }

        public HomeController(TicketContext context)
        {
            ApplicationContext = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await ApplicationContext.Events.ToListAsync());
        }

        public IActionResult PrototypeIndex()
        {
            return View();
        }

        public IActionResult NoTicketDesign()
        {
            return View();
        }

        public IActionResult Purchaser()
        {
            var viewModel = new PurchaserViewModel
            {
                Email = string.Empty,
                Name = string.Empty
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Purchaser(PurchaserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var purchaser = ApplicationContext.TicketPurchasers
                    .AsNoTracking()
                    .Include(purchaser0 => purchaser0.Tickets)
                    .FirstOrDefault(ticketPurchaser => ticketPurchaser.Email == viewModel.Email);

                if (purchaser == null)
                {
                    // Display message that there are no tickets for the e-mail given.
                    return View(viewModel);
                }
                else
                {
                    return RedirectToAction(nameof(Ready), new { purchaserId = purchaser.Id });
                }
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Purchase(int? eventId)
        {
            var firstOrDefaultTicketDesign = ApplicationContext.TicketDesigns.FirstOrDefaultAsync(ticketDesign => ticketDesign.EventId == eventId).Result;

            if (eventId == null)
            {
                return NotFound();
            }

            if (firstOrDefaultTicketDesign == null)
            {
                return RedirectToAction(nameof(NoTicketDesign));
            }

            return await PurchaseInternal(eventId.Value);
        }

        private async Task<IActionResult> PurchaseInternal(int eventId, PurchaserViewModel purchaser = null)
        {
            var purchaseEvent = await ApplicationContext.Events.SingleOrDefaultAsync(event0 => event0.Id == eventId);

            PurchaseViewModel viewModel = new PurchaseViewModel()
            {
                EventId = purchaseEvent.Id,
                EventName = purchaseEvent.Name,
                Location = purchaseEvent.Location,
                EventStart = purchaseEvent.StartTime.ToString("HH:mm"),
                EventEnd = purchaseEvent.EndTime.ToString("HH:mm")
            };

            if (purchaser == null)
            {
                purchaser = new PurchaserViewModel
                {
                    Name = string.Empty,
                    Email = string.Empty,
                };
            }

            viewModel.Purchaser = purchaser;
            viewModel.EventTickets = MakeEventTicketViewModels(purchaseEvent.Id);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Purchase(int eventId, IList<TicketTypeViewModel> ticketTypes, PurchaseViewModel viewModel)
        {
            var ticketsToPurchase = ticketTypes.Sum(ticketType => ticketType.Quantity);

            if (ticketsToPurchase == 0)
            {
                ModelState.AddModelError(string.Empty, "Quantity can't be zero.");
            }

            var purchaseEvent = ApplicationContext.Events.SingleOrDefault(event0 => event0.Id == eventId);
            var eventTickets = MakeEventTickets(eventId);

            foreach (var eventTicket in eventTickets)
            {
                foreach (var ticketType in ticketTypes)
                {
                    if (eventTicket.TicketTypeId == ticketType.Id)
                    {
                        if (eventTicket.QuantityAvailable < ticketType.Quantity)
                        {
                            ModelState.AddModelError(string.Empty, "There cannot be tickets of a type than what is availiable");
                            break;
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                var purchaserEmail = viewModel.Purchaser.Email;
                var puchaserName = viewModel.Purchaser.Name;

                var ticketPurchasersWithEmail = ApplicationContext.TicketPurchasers.FirstOrDefault(ticketPurchaser => ticketPurchaser.Name == puchaserName);

                if (ticketPurchasersWithEmail == null)
                {
                    ticketPurchasersWithEmail = new TicketPurchaser
                    {
                        Name = puchaserName,
                        Email = purchaserEmail
                    };
                    ApplicationContext.TicketPurchasers.Add(ticketPurchasersWithEmail);
                }

                await ApplicationContext.SaveChangesAsync();

                var purchaserId = ticketPurchasersWithEmail.Id;
                var tickets = CreateTickets(ticketTypes, purchaserId, eventTickets).ToList();

                ApplicationContext.Tickets.AddRange(tickets);

                // There must be a much better way to update ticket quantity
                foreach (var eventTicket in eventTickets)
                {
                    var ticketTypeQuanity = ticketTypes.FirstOrDefault(ticketType => ticketType.Id == eventTicket.TicketTypeId).Quantity;
                    eventTicket.QuantityAvailable -= ticketTypeQuanity;

                    try
                    {
                        ApplicationContext.EventTickets.Update(eventTicket);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        var eventTicketExists = ApplicationContext.EventTickets.Any(eventTicket0 => eventTicket0.Id == eventTicket.Id);
                        if (!eventTicketExists)
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                await ApplicationContext.SaveChangesAsync();

                return RedirectToAction(nameof(Ready), new { purchaserId });
            }

            return await PurchaseInternal(eventId, viewModel.Purchaser);
        }

        private static IEnumerable<Models.Ticket> CreateTickets(IEnumerable<TicketTypeViewModel> ticketTypes, int purchaserId, IEnumerable<EventTicket> eventTickets)
        {
            var now = DateTimeOffset.Now;

            foreach (var ticketType in ticketTypes)
            {
                var eventTicket = eventTickets.SingleOrDefault(eventTicket0 => eventTicket0.TicketTypeId == ticketType.Id);
                var eventTicketId = eventTicket.Id;
                var amountPaid = eventTicket.Price;

                for (int quantityPosition = 0; quantityPosition < ticketType.Quantity; quantityPosition++)
                {
                    var ticket = new Models.Ticket
                    {
                        EventTicketId = eventTicketId,
                        TicketPurchaserId = purchaserId,
                        AmountPaid = amountPaid,
                        DateSold = now,
                        AttendeeName = string.Empty
                    };
                    yield return ticket;
                }
            }
        }

        public IActionResult Ready(int purchaserId)
        {
            var viewModel = MakeReadyAndPrintViewModel(purchaserId);
            return View(viewModel);
        }

        private ReadyAndPrintViewModel MakeReadyAndPrintViewModel(int purchaserId)
        {
            var tickets = ApplicationContext.Tickets
                .AsNoTracking()
                .Include(ticket => ticket.EventTicket)
                    .ThenInclude(eventTicket => eventTicket.TicketType)
                .Include(ticket => ticket.EventTicket)
                    .ThenInclude(eventTicket => eventTicket.TicketDesign)
                    .ThenInclude(ticketDesign => ticketDesign.Event)
                .Include(ticket => ticket.TicketPurchaser)
                .Include(ticket => ticket.TicketStatus)
                .Where(ticket => ticket.TicketPurchaserId == purchaserId)
                .ToList();

            var ticketViewModels =
                from ticket in tickets
                select new TicketViewModel
                {
                    TicketId = ticket.Id,
                    Event = ticket.EventTicket.TicketDesign.Event.Name,
                    AmountPaid = ticket.AmountPaid.ToThaiCurrencyDisplayString(),
                    DateSold = ticket.DateSold.ToString("MMMM dd, yyyy"),
                    AttendeeName = ticket.AttendeeName,
                    TicketType = ticket.EventTicket.TicketType.Name,
                    Number = ticket.Number,
                    TicketPurchaser = ticket.TicketPurchaser.Name
                };

            var viewModel = new ReadyAndPrintViewModel
            {
                PurchaserId = purchaserId,
                TicketViewModels = ticketViewModels.ToList()
            };

            return viewModel;
        }

        [HttpPost]
        public IActionResult Ready(ReadyAndPrintViewModel viewModel)
        {
            foreach (var ticketViewModel in viewModel.TicketViewModels)
            {
                var ticket = ApplicationContext.Tickets.SingleOrDefault(ticket0 => ticket0.Id == ticketViewModel.TicketId);
                ticket.AttendeeName = ticketViewModel.AttendeeName;
                ApplicationContext.Update(ticket);
            }

            ApplicationContext.SaveChanges();

            return RedirectToAction(nameof(Print), new { viewModel.PurchaserId });
        }

        public IActionResult Print(int purchaserId)
        {
            var viewModel = MakeReadyAndPrintViewModel(purchaserId);
            return View(viewModel);
        }

        /// <summary>
        /// For working with the data for the Event Tickets of the event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        private IEnumerable<EventTicket> MakeEventTickets(int eventId)
        {
            var ticketDesign = ApplicationContext.TicketDesigns
                .AsNoTracking()
                .SingleOrDefaultAsync(ticketDesign0 => ticketDesign0.EventId == eventId)
                .Result;

            var eventTicketsQuery =
                from eventTicket in ApplicationContext.EventTickets
                    .AsNoTracking()
                    .Include(eventTicket0 => eventTicket0.TicketType)
                    .Include(eventTicket0 => eventTicket0.TicketDesign)
                where eventTicket.TicketDesignId == ticketDesign.Id
                select eventTicket;

            return eventTicketsQuery;
        }

        /// <summary>
        /// For dispalying the data for the event tickets of the event 
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        private IEnumerable<EventTicketViewModel> MakeEventTicketViewModels(int eventId)
        {
            var eventTickets = MakeEventTickets(eventId).ToList();

            return
                from eventTicket in eventTickets
                select new EventTicketViewModel
                {
                    Id = eventTicket.Id,
                    TicketDesign = eventTicket.TicketDesign.Name,
                    TicketType = eventTicket.TicketType.Name,
                    QuantityAvailable = eventTicket.QuantityAvailable.ToString("G", new CultureInfo("th-TH")),
                    Price = eventTicket.Price.ToThaiCurrencyDisplayString()
                };
        }
    }
}

