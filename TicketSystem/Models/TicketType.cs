using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Models
{
    public class TicketType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(128, ErrorMessage = "Must be 128 characters or less")]
        public string Name { get; set; }

        public ICollection<EventTicket> EventTickets { get; set; }
    }
}
