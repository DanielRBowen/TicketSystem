using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSystem.Models
{
    public class TicketDesignElement
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("TicketDesign")]
        [Display(Name = "Ticket Design")]
        public int TicketDesignId { get; set; }

        public TicketDesign TicketDesign { get; set; }

        [MaxLength(64, ErrorMessage = "Must be 64 characters or less")]
        public string Name { get; set; }

        public int XCoordinate { get; set; }

        public int YCoordinate { get; set; }

        public int ZIndex { get; set; }

        public int XDimension { get; set; }

        public int YDimension { get; set; }

        [MaxLength(128, ErrorMessage = "Must be 128 characters or less")]
        public string Content { get; set; }
    }
}
