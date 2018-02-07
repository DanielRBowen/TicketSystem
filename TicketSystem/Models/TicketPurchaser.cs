using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Models
{
    public class TicketPurchaser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "Must be 64 characters or less")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Must be an E-mail address")]
        public string Email { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
