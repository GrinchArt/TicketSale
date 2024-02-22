using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketSale.Domain.Models
{
    public class Route
    {
        public int Id { get; set; }
        public string PlaceOfDeparture { get; set; }
        public string PlaceOfDestination { get; set; }
        public int TransportTypeId { get; set; }
    }
}
