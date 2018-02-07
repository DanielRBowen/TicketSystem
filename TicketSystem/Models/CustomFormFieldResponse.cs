using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystem.Models
{
    public class CustomFormFieldResponse
    {
        [Key]
        public int Id { get; set; }

        public int TicketId { get; set; }

        public Ticket Ticket { get; set; }

        [Display(Name = "Custom Form Field Question")]
        public int CustomFormFieldQuestionId { get; set; }

        public CustomFormFieldQuestion CustomFormFieldQuestion { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "Must be 255 characters or less")]
        public string Value { get; set; }
    }
}
