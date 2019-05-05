using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HotelReservation.Data.ViewModels.CustomerViewModels
{
    public class AddUpdateCustomerViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Numer dowodu tożsamości jest wymagany")]
        public string IDNumber { get; set; }

        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Wpisz poprawny adres email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Numer telefonu jest wymagany")]
        [RegularExpression(@"\d{9}", ErrorMessage = "Wprowadź poprawny numer telefonu składający sie z 9 cyfr")]
        public int? Phone { get; set; }
    }
}
