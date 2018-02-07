using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystem.Models
{
    public class TicketDesign
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Event")]
        [Display(Name = "Event")]
        public int EventId { get; set; }

        public Event Event { get; set; }

        [MaxLength(64, ErrorMessage = "Must be 64 characters or less")]
        public string Name { get; set; }

        [MaxLength(255, ErrorMessage = "Must be 255 characters or less")]
        public string Description { get; set; }

        public ICollection<TicketDesignElement> TicketDesignElements { get; set; }

        public ICollection<EventTicket> EventTickets { get; set; }

        public ICollection<CustomFormFieldQuestion> CustomFormFieldQuestions { get; set; }
    }
}
