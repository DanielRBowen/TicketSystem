using System.Collections.Generic;

namespace TicketSystem.ViewModels
{
    public class PurchaseViewModel
    {
        public int EventId { get; set; }

        public string EventName { get; set; }

        public string EventEnd { get; set; }

        public string EventStart { get; set; }

        public string Location { get; set; }

        public PurchaserViewModel Purchaser { get; set; }

        public IEnumerable<EventTicketViewModel> EventTickets { get; set; }

    }
}
