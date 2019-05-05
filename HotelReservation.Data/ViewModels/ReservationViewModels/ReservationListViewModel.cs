using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HotelReservation.Data.ViewModels.ReservationViewModels
{
    public class ReservationListViewModel
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public string Hotel { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime AccommodationDate { get; set; }
    }
}
