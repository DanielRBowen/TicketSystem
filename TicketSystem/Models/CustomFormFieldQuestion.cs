using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystem.Models
{
    public class CustomFormFieldQuestion
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Ticket Design")]
        public int TicketDesignId { get; set; }

        public TicketDesign TicketDesign { get; set; }

        [Required]
        [Display(Name = "Custom Form Field Datatype")]
        public int CustomFormFieldDatatypeId { get; set; }
        
        [Display(Name ="Data Type")]
        public CustomFormFieldDatatype CustomFormFieldDatatype { get; set; }

        [Required]
        [MaxLength(128, ErrorMessage = "Must be 128 characters or less")]
        public string Label { get; set; }

        [Required]
        public bool Required { get; set; }

        public ICollection<CustomFormFieldDataOption> CustomFormFieldDataOptions { get; set; }

        public ICollection<CustomFormFieldResponse> CustomFormFieldResponses { get; set; }
    }
}
