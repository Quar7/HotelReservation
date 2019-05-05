using AutoMapper;
using HotelReservation.Data.Entities;
using HotelReservation.Data.ViewModels.CustomerViewModels;
using HotelReservation.Data.ViewModels.HotelViewModels;
using HotelReservation.Data.ViewModels.ReservationViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Hotel, HotelViewModel>();
            CreateMap<AddUpdateHotelViewModel, Hotel>()
                .ForMember(d => d.Id, opt => opt.Ignore());

            CreateMap<Customer, CustomerViewModel>();
            CreateMap<AddUpdateCustomerViewModel, Customer>()
                .ForMember(d => d.Id, opt => opt.Ignore());

            CreateMap<Reservation, ReservationListViewModel>()
                .ForMember(d => d.Customer, opt => opt.MapFrom(s => s.Customer.FirstName + " " + s.Customer.LastName))
                .ForMember(d => d.Hotel, opt => opt.MapFrom(s => s.Hotel.Name));
            CreateMap<Reservation, ReservationDetailsViewModel>();
            CreateMap<AddReservationViewModel, Reservation>()
                .ForMember(d => d.Id, opt => opt.Ignore());
            CreateMap<UpdateReservationViewModel, Reservation>()
                .ForMember(d => d.Id, opt => opt.Ignore());
        }
    }
}
