using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Data.Entities
{
    public class Reservation : BaseEntity
    {
        public int CustomerId { get; set; }
        public int HotelId { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime AccommodationDate { get; set; }

        public Customer Customer { get; set; }
        public Hotel Hotel { get; set; }
    }
}
