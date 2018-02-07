using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TicketSystem.Models
{
    public class Ticket
    {
        private const ulong A00000InBase36 = 604661760;

        private static string ToBase36(ulong value)
        {
            const string base36Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var buffer = new StringBuilder(13);

            do
            {
                var index = (int)(value % 36);
                buffer.Insert(0, base36Digits[index]);
                value /= 36;
            } while (value != 0);

            return buffer.ToString();
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = "Event Ticket")]
        public int EventTicketId { get; set; }

        public virtual EventTicket EventTicket { get; set; }

        [Display(Name = "Ticket Purchaser")]
        public int TicketPurchaserId { get; set; }

        public virtual TicketPurchaser TicketPurchaser { get; set; }

        [Display(Name = "Ticket Status")]
        public int? TicketStatusId { get; set; }

        public virtual TicketStatus TicketStatus { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal AmountPaid { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date Sold")]
        public DateTimeOffset DateSold { get; set; }

        [Display(Name = "Attendee Name")]
        [MaxLength(128, ErrorMessage = "Must be 128 characters or less")]
        public string AttendeeName { get; set; }

        [NotMapped]
        public string Number => ToBase36(A00000InBase36 + (ulong)Id);

        public ICollection<CustomFormFieldResponse> CustomFormFieldResponses { get; set; }
    }
}
