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
    public class TicketEntityConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Price)
                .IsRequired();
            builder.Property(t => t.Price)
               .HasPrecision(18, 2);

            builder.Property(t => t.IsOneWay)
                .IsRequired();

            builder.Property(t => t.IsWithPet)
               .IsRequired();

            builder.Property(t => t.IsWithBaggage)
               .IsRequired();

            builder.Property(t => t.SeatNumber)
               .IsRequired()
               .HasMaxLength(10);

            builder.HasOne<Schedule>()
               .WithMany()
               .HasForeignKey(t => t.ScheduleId);

            builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(t => t.UserId);
        }
    }
}
