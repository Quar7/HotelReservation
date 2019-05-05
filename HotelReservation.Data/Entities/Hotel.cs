using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Data.Entities
{
    public class Hotel : BaseEntity
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int Rating { get; set; }
        public int AvailableRooms { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
