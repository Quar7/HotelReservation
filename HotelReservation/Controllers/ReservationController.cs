using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Repositories.Interfaces;
using HotelReservation.Data.ViewModels.ReservationViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Controllers
{
    [Route("api/[controller]")]
    public class ReservationController : BaseController
    {
        private IReservationRepository _reservationRepository;
        private IHotelRepository _hotelRepository;
        private ICustomerRepository _customerRepository;
        private string _message;

        public ReservationController(IReservationRepository reservationRepository, IHotelRepository hotelRepository, ICustomerRepository customerRepository, IMapper mapper) : base(mapper)
        {
            _reservationRepository = reservationRepository;
            _hotelRepository = hotelRepository;
            _customerRepository = customerRepository;
        }

        // GET: api/reservation
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var reservationEntities = await _reservationRepository.GetAll();
                if (reservationEntities.Any())
                {
                    var reservations = _mapper.Map<IEnumerable<Reservation>, IEnumerable<ReservationListViewModel>>(reservationEntities);
                    return Ok(reservations);
                }
                else
                {
                    _message = "Baza danych rezerwacji jest pusta";
                    return NotFound(_message);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/reservation/id
        [HttpGet("{id}", Name = "GetReservation")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var reservationEntity = await _reservationRepository.GetEntityById(id);
                if (reservationEntity != null)
                {
                    var reservation = _mapper.Map<Reservation, ReservationDetailsViewModel>(reservationEntity);
                    return Ok(reservation);
                }
                else
                {
                    _message = $"Rezerwacja o identyfikatorze {id} nie istnieje";
                    return NotFound(_message);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //POST: api/reservation
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddReservationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var statusCode = await ValidateEntities(model);
                if (statusCode == HttpStatusCode.NotFound)
                {
                    return NotFound(_message);
                }
                else if (statusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest(_message);
                }

                model.ReservationDate = DateTime.Now.Date;
                var newReservationEntity = _mapper.Map<AddReservationViewModel, Reservation>(model);
                _reservationRepository.AddEntity(newReservationEntity);
                await _hotelRepository.DecreaseAvailableRoomQuantity((int)model.HotelId);

                if (await _reservationRepository.SaveAsync())
                {
                    model.Id = newReservationEntity.Id;
                    var newUri = Url.Link("GetReservation", new { id = model.Id });
                    return Created(newUri, model);
                }
                else
                {
                    _message = "Nie udało się dodać nowej rezerwacji";
                    return BadRequest(_message);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //PUT: api/reservation/id
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateReservationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var reservationEntity = await _reservationRepository.GetEntityById(id);
                if (reservationEntity == null)
                {
                    _message = $"Rezerwacja o identyfikatorze {id} nie istnieje";
                    return NotFound(_message);
                }

                var statusCode = await ValidateEntities(model);
                if (statusCode == HttpStatusCode.NotFound)
                {
                    return NotFound(_message);
                }
                else if (statusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest(_message);
                }

                if (model.HotelId != reservationEntity.HotelId)
                {
                    await _hotelRepository.IncreaseAvailableRoomQuantity(reservationEntity.HotelId);
                    await _hotelRepository.DecreaseAvailableRoomQuantity((int)model.HotelId);
                }

                _mapper.Map(model, reservationEntity);
                _reservationRepository.UpdateEntity(reservationEntity);

                if (await _reservationRepository.SaveAsync())
                {
                    var updatedReservation = _mapper.Map<Reservation, UpdateReservationViewModel>(reservationEntity);
                    return Ok(updatedReservation);
                }
                else
                {
                    _message = "Nie udało się zaktualizować rezerwacji";
                    return BadRequest(_message);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //DELETE: api/reservation/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var reservationEntity = await _reservationRepository.GetEntityById(id);
                if (reservationEntity == null)
                {
                    _message = $"Rezerwacja o identyfikatorze {id} nie istnieje";
                    return NotFound(_message);
                }

                _reservationRepository.DeactivateEntity(reservationEntity);
                await _hotelRepository.IncreaseAvailableRoomQuantity(reservationEntity.HotelId);

                if (await _reservationRepository.SaveAsync())
                {
                    _message = $"Usunięto rezerwację o identyfikatorze {id}";
                    return Ok(_message);
                }
                else
                {
                    _message = "Nie udało się usunąć rezerwacji";
                    return BadRequest(_message);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Private methods
        #region Private methods

        private async Task<HttpStatusCode> ValidateEntities(AddUpdateReservationViewModel model)
        {
            var statusCode = HttpStatusCode.OK;
            var customer = await _customerRepository.GetEntityById((int)model.CustomerId);
            if (customer == null)
            {
                _message += $"Klient o identyfikatorze {model.CustomerId} nie istnieje \n";
                statusCode = HttpStatusCode.NotFound;
            }

            var hotel = await _hotelRepository.GetEntityById((int)model.HotelId);
            if (hotel == null)
            {
                _message += $"Hotel o identyfikatorze {model.HotelId} nie istnieje \n";
                statusCode = HttpStatusCode.NotFound;
            }
            else if (hotel.AvailableRooms < 1)
            {
                _message += $"W hotelu \"{hotel.Name}\" obecnie nie ma wolnych pokoi \n";
                statusCode = HttpStatusCode.BadRequest;
            }

            return statusCode;
        }
        #endregion
    }
}
