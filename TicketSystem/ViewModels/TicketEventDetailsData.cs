using TicketSystem.Models;
using System.Collections.Generic;

namespace TicketSystem.ViewModels
{
    public class TicketEventDetailsData
	{
		public Event TicketEvent { get; set; }
        public IEnumerable<EventTicket> EventTickets { get; set; }
        public IList<Models.Ticket> Tickets { get; set; } = new List<Models.Ticket>();
    }
}
