using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicketSale.Domain;

namespace TicketSale.Infrastructure.Data.EntityTypeConfiguration
{
    public class RouteEntityConfiguration : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.CountryOfDeparture)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(r => r.PlaceOfDeparture)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(r => r.CountryOfDestination)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(r => r.PlaceOfDestination)
               .IsRequired()
               .HasMaxLength(255);

            builder.Property(r => r.TransportTypeId)
                .IsRequired();

            builder.HasOne<TransportType>()
               .WithMany()
               .HasForeignKey(r => r.TransportTypeId);
        }
    }
}
