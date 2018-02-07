using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystem.Models
{
    public class CustomFormFieldDataOption
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("CustomFormFieldQuestion")]
        [Display(Name = "Custom Form Field Question")]
        public int CustomFormFieldQuestionId { get; set; }

        public virtual CustomFormFieldQuestion CustomFormFieldQuestion { get; set; }

        [MaxLength(64, ErrorMessage = "Must be 64 characters or less")]
        public string Value { get; set; }
    }
}
