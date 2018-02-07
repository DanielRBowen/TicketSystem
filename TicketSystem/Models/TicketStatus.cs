using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Models
{
    public class TicketStatus
    {
        [Key]
        public int Id { get; set; }

        [StringLength(32)]
        public string Name { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
