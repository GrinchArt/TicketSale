using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketSale.Domain;


namespace TicketSale.Infrastructure.Data.EntityTypeConfiguration
{
    public class BookingStatusEntityConfiguration : IEntityTypeConfiguration<BookingStatus>
    {
        public void Configure(EntityTypeBuilder<BookingStatus> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}

