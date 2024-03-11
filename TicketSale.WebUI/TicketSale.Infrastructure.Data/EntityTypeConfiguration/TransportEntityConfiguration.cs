using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicketSale.Domain;

namespace TicketSale.Infrastructure.Data.EntityTypeConfiguration
{
    public class TransportEntityConfiguration : IEntityTypeConfiguration<TransportType>
    {
        public void Configure(EntityTypeBuilder<TransportType> builder)
        {

        }
    }
}
