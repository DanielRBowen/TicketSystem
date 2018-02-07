using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace TicketSystem.Data
{
    public class TicketContext : DbContext
    {
        public TicketContext(DbContextOptions<TicketContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
        }

        // Ticket Models
        public DbSet<Event> Events { get; set; }

        public DbSet<TicketPurchaser> TicketPurchasers { get; set; }

        public DbSet<EventTicket> EventTickets { get; set; }

        public DbSet<TicketType> TicketTypes { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<TicketStatus> TicketStatuses { get; set; }

        public DbSet<TicketDesign> TicketDesigns { get; set; }

        public DbSet<TicketDesignElement> TickedDesignElements { get; set; }

        public DbSet<CustomFormFieldQuestion> CustomFormFieldQuestions { get; set; }

        public DbSet<CustomFormFieldResponse> CustomFormFieldResponses { get; set; }

        public DbSet<CustomFormFieldDataOption> CustomFormFieldDataOptions { get; set; }

        public DbSet<CustomFormFieldDatatype> CustomFormFieldDatatypes { get; set; }
    }
}
