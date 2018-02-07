using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Models
{
    public class CustomFormFieldDatatype
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        public ICollection<CustomFormFieldQuestion> CustomFormFieldQuestion { get; set; }
    }
}
