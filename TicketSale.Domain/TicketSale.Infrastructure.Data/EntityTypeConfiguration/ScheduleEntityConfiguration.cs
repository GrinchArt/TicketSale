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
    public class ScheduleEntityConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.RouteId)
                .IsRequired();

            builder.Property(s => s.DepartureTime)
                .IsRequired();

            builder.Property(s => s.ArrivalTime)
               .IsRequired();

            builder.Property(s => s.AvailableSeats)
               .IsRequired();

            builder.HasOne<Route>()
               .WithMany()
               .HasForeignKey(s => s.RouteId);
        }
    }
}
