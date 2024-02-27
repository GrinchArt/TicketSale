using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketSale.Domain.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public bool IsOneWay { get; set; }
        public bool IsWithPet { get; set; }
        public bool IsWithBaggage { get; set; }
        public string SeatNumber { get; set; }

        public int ScheduleId { get; set; }
        public int UserId { get; set; }
    }
}
