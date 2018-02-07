using System;
using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(128, ErrorMessage = "Must be 128 characters or less")]
        public string Name { get; set; }

        [StringLength(128)]
        public string Location { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime BeginSales { get; set; }

        public DateTime EndSales { get; set; }

        public bool TicketSalesEnabled { get; set; }
    }
}
