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
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id); 

            builder.Property(u => u.Name)
                .IsRequired() 
                .HasMaxLength(50); 

            builder.Property(u => u.SurName)
                .IsRequired() 
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .IsRequired() 
                .HasMaxLength(100); 
            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20); 

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255); 

            builder.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}
