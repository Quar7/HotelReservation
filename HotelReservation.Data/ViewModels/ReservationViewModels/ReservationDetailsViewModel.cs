using HotelReservation.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HotelReservation.Data.ViewModels.ReservationViewModels
{
    public class ReservationDetailsViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int HotelId { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime AccommodationDate { get; set; }

        public Customer Customer { get; set; }
        public Hotel Hotel { get; set; }
    }
}
