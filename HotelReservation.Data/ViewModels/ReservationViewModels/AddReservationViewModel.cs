using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HotelReservation.Data.ViewModels.ReservationViewModels
{
    public class AddReservationViewModel : AddUpdateReservationViewModel
    {
        public DateTime ReservationDate { get; set; }
    }
}
