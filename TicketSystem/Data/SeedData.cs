using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.Models;

namespace TicketSystem.Data
{
    public class SeedData
    {
        public static void Initialize(TicketContext context)
        {
            context.Database.EnsureCreated();

            if (context.Events.Any())
            {
                return;
            }

            // Password: Test1@
            //var applicationUser = new ApplicationUser { Email = "test@test.com", PasswordHash = @"AQAAAAEAACcQAAAAEKP4gbCRL1R5NuTpBgxX6NeVHdawzhFCsH/ip5qkwv8lRAWEkLOYKuCcC7PMCmFDCQ==" };
            //context.ApplicationUser.Add(applicationUser);

            // Events
            var ticketEvents = new Event[]
            {
                new Event { Name = "Event of August", Location = "Example Street, Example City, Example, 99999", StartTime = new DateTime(2017, 8, 15, 18, 0, 0), EndTime = new DateTime(2018, 8, 15, 22, 0, 0) },
                new Event { Name = "Event of September", Location = "Example Street, Example City, Example, 99999", StartTime = new DateTime(2017, 9, 15, 18, 0, 0), EndTime = new DateTime(2018, 9, 15, 22, 0, 0) },
                new Event { Name = "Event of October", Location = "Example Street, Example City, Example, 99999", StartTime = new DateTime(2017, 10, 15, 18, 0, 0), EndTime = new DateTime(2018, 10, 15, 22, 0, 0) }
            };
            foreach (Event ticketEvent in ticketEvents)
            {
                context.Events.Add(ticketEvent);
            }

            //TicketTypes
            var ticketTypes = new TicketType[]
            {
                new TicketType { Name = "Adult" },
                new TicketType { Name = "Child" },
                new TicketType { Name = "VIP" }
            };
            foreach (TicketType ticketType in ticketTypes)
            {
                context.TicketTypes.Add(ticketType);
            }
            context.SaveChanges(); //Need to save everytime if using Ids in further additions

            var ticketDesigns = new TicketDesign[]
            {
                new TicketDesign { EventId = 1, Name = "Waves", Description = "A ticket with a picture of waves in the background and asks if the attendee can swim." },
                new TicketDesign { EventId = 2, Name = "Stars", Description = "A ticket with a picture of stars in the background and asks if the attendee is vegan." }
            };
            foreach (var ticketDesign in ticketDesigns)
            {
                context.TicketDesigns.Add(ticketDesign);
            }
            context.SaveChanges();

            // 33 Baht is around a dollar
            var eventTickets = new EventTicket[]
            {
                new EventTicket { TicketDesignId = 1, TicketTypeId = 1, QuantityAvailable = 50, Price = 99 },
                new EventTicket { TicketDesignId = 1, TicketTypeId = 2, QuantityAvailable = 15, Price = 33 },
                new EventTicket { TicketDesignId = 1, TicketTypeId = 3, QuantityAvailable = 7, Price = 330 }
            };
            foreach (EventTicket eventTicket in eventTickets)
            {
                context.EventTickets.Add(eventTicket);
            }

            // Tickets Statuses
            var ticketStatuses = new TicketStatus[]
            {
                new TicketStatus { Name = "Active" },
                new TicketStatus { Name = "Expired" }
            };
            foreach (TicketStatus ticketStatus in ticketStatuses)
            {
                context.Add(ticketStatus);
            }
            context.SaveChanges();

            // Tickets
            //var tickets = new Ticket[]
            //{
            //    new Ticket { }
            //};
            //foreach (Ticket ticket in tickets)
            //{
            //    context.Add(ticket);
            //}


            //Data Types
            var customFormFieldDataTypes = new CustomFormFieldDatatype[]
            {
                new CustomFormFieldDatatype { Name = "Text" },
                //new CustomFormFieldDatatype { Name = "Dropdown" },
                //new CustomFormFieldDatatype { Name = "Number" }
            };
            foreach (var dataTypeName in customFormFieldDataTypes)
            {
                context.Add(dataTypeName);
            }
            context.SaveChanges();

            var customFormFieldQuestions = new CustomFormFieldQuestion[]
            {
                new CustomFormFieldQuestion { TicketDesignId = 1, CustomFormFieldDatatypeId = 1, Label = "Are you on a Ketogenic diet?", Required = true},
                new CustomFormFieldQuestion { TicketDesignId = 2, CustomFormFieldDatatypeId = 1, Label = "Please list any food allergies", Required = true}
            };
            foreach (var customFormFieldQuestion in customFormFieldQuestions)
            {
                context.Add(customFormFieldQuestion);
            }
            context.SaveChanges();

            /* 
             var customCustomFormFieldDataOptions = new CustomFormFieldDataOption[]
             {
                 new CustomFormFieldDataOption { CustomFormFieldQuestionId = 1, Value = "Yes"},
                 new CustomFormFieldDataOption { CustomFormFieldQuestionId = 1, Value = "No"}
             };
             foreach (var customFormFieldDataOption in customCustomFormFieldDataOptions)
             {
                 context.Add(customFormFieldDataOption);
             }
             context.SaveChanges();
             */
        }
    }
}
