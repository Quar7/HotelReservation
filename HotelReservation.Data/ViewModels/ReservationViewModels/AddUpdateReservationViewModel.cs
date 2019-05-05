using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HotelReservation.Data.ViewModels.ReservationViewModels
{
    public abstract class AddUpdateReservationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Numer klienta jest wymagany")]
        public int? CustomerId { get; set; }

        [Required(ErrorMessage = "Numer hotelu jest wymagany")]
        public int? HotelId { get; set; }

        [Required(ErrorMessage = "Data zakwaterowania jest wymagana")]
        public DateTime AccommodationDate { get; set; }
    }
}
