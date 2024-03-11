using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketSale.Domain;

namespace TicketSale.Infrastructure.Data.EntityTypeConfiguration
{
    public class BookingEntityConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.TicketId)
            .IsRequired();

            builder.HasOne<Ticket>()
                .WithMany()
                .HasForeignKey(b => b.TicketId);

            builder.Property(b => b.BookingStatusId)
               .IsRequired();

            builder.HasOne<BookingStatus>()
                .WithMany()
                .HasForeignKey(b => b.BookingStatusId);
        }
    }
}
