using TicketSystem.Models;
using System.Collections.Generic;

namespace TicketSystem.ViewModels
{
    public class EventTicketPurchaseData
    {
        public Event Event { get; set; }

        public TicketDesign TicketDesign { get; set; }

        public IEnumerable<EventTicket> EventTickets { get; set; }
    }
}
