using EventMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventMS.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TypeTicket> TypeTickets { get; set; }
        public DbSet<EventRegistration> EventRegistrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>(ConfigureEvent);
            modelBuilder.Entity<Ticket>(ConfigureTicket);
            modelBuilder.Entity<User>(ConfigureUser);
            modelBuilder.Entity<TypeTicket>(ConfigureTypeTicket);
            modelBuilder.Entity<EventRegistration>()
                 .HasKey(e => e.Id);
        }

        private void ConfigureEvent(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Title).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(500);
            builder.Property(e => e.Date).IsRequired();
            builder.Property(e => e.Time).IsRequired();
            builder.Property(e => e.Location).IsRequired();

            builder.HasMany(e => e.TypeTickets)
                   .WithOne(tt => tt.Event)
                   .HasForeignKey(tt => tt.EventId)
                   .OnDelete(DeleteBehavior.Cascade); 

            builder.HasMany(e => e.EventRegistrations)
                   .WithOne(er => er.Event)
                   .HasForeignKey(er => er.EventId)
                   .OnDelete(DeleteBehavior.Cascade); 
        }

        private void ConfigureTicket(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.PurchaseDate).IsRequired();

            builder.HasOne(t => t.User)
                   .WithMany(u => u.Tickets)
                   .HasForeignKey(t => t.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.TypeTicket)
                   .WithMany(tt => tt.Tickets)
                   .HasForeignKey(t => t.TypeTicketId)
                   .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureUser(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Name).IsRequired();
            builder.Property(u => u.Surname).IsRequired();
            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.Nickname).IsRequired();
            builder.Property(u => u.Role).IsRequired();

            builder.HasMany(u => u.Tickets)
                   .WithOne(t => t.User)
                   .HasForeignKey(t => t.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }


        private void ConfigureTypeTicket(EntityTypeBuilder<TypeTicket> builder)
        {
            builder.HasKey(tt => tt.Id);
            builder.Property(tt => tt.Name).IsRequired();
            builder.Property(tt => tt.Description).IsRequired();
            builder.Property(tt => tt.Price)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.Property(tt => tt.QuantityAvailable).IsRequired();

            builder.HasMany(tt => tt.Tickets)
                   .WithOne(t => t.TypeTicket)
                   .HasForeignKey(t => t.TypeTicketId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(tt => tt.Event)
                   .WithMany(e => e.TypeTickets)
                   .HasForeignKey(tt => tt.EventId)
                   .OnDelete(DeleteBehavior.Cascade); // Cambiado a Cascade
        }

        private void ConfigureEventRegistration(EntityTypeBuilder<EventRegistration> builder)
        {
            builder.HasKey(er => er.Id);
            builder.HasIndex(er => new { er.UserId, er.EventId }).IsUnique();

            builder.HasOne(er => er.User)
                   .WithMany(u => u.EventRegistrations)
                   .HasForeignKey(er => er.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(er => er.Event)
                   .WithMany(e => e.EventRegistrations)
                   .HasForeignKey(er => er.EventId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}