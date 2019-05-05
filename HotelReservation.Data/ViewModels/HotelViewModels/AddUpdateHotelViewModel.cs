using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HotelReservation.Data.ViewModels.HotelViewModels
{
    public class AddUpdateHotelViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Miasto jest wymagane")]
        public string City { get; set; }

        [Required(ErrorMessage = "Adres jest wymagany")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Klasyfikacja jest wymagana")]
        [Range(1, 5, ErrorMessage = "Klasyfikacja hotelu musi się mieścić w przedziale od 1 do 5")]
        public int? Rating { get; set; }

        [Required(ErrorMessage = "Liczba dostępnych pokoi jest wymagana")]
        public int? AvailableRooms { get; set; }
    }
}
