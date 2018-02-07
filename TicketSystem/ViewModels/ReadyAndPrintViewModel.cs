using System.Collections.Generic;

namespace TicketSystem.ViewModels
{
    public class ReadyAndPrintViewModel
    {
        public int PurchaserId { get; set; }

        public IList<TicketViewModel> TicketViewModels { get; set; }
    }
}
