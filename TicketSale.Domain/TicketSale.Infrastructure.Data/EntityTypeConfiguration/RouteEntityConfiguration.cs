using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketSale.Domain.Models;

namespace TicketSale.Infrastructure.Data.EntityTypeConfiguration
{
    public class RouteEntityConfiguration : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.PlaceOfDeparture)
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
