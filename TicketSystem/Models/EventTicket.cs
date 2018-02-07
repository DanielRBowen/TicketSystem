using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystem.Models
{
    public class EventTicket
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("TicketDesign")]
        [Display(Name = "Ticket Design")]
        public int TicketDesignId { get; set; }

        public TicketDesign TicketDesign { get; set; }

        [Required]
        [ForeignKey("TicketType")]
        [Display(Name = "Ticket Type")]
        public int TicketTypeId { get; set; }

        public TicketType TicketType { get; set; }

        [Required]
        public int QuantityAvailable { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName ="money")]
        public decimal Price { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
