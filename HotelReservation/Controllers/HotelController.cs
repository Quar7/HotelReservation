using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using HotelReservation.Data.ViewModels.HotelViewModels;
using HotelReservation.Data.Entities;
using System.Linq;
using HotelReservation.Data.Repositories.Interfaces;

namespace HotelReservation.Controllers
{
    [Route("api/[controller]")]
    public class HotelController : BaseController
    {
        private IHotelRepository _hotelRepository;
        private string _message;

        public HotelController(IHotelRepository hotelRepository, IMapper mapper) : base(mapper)
        {
            _hotelRepository = hotelRepository;
        }

        // GET: api/hotel
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var hotelEntities = await _hotelRepository.GetAll();
                if (hotelEntities.Any())
                {
                    var hotels = _mapper.Map<IEnumerable<Hotel>, IEnumerable<HotelViewModel>>(hotelEntities);
                    return Ok(hotels);
                }
                else
                {
                    _message = "Baza danych hoteli jest pusta";
                    return NotFound(_message);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/hotel/id
        [HttpGet("{id}", Name = "GetHotel")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var hotelEntity = await _hotelRepository.GetEntityById(id);
                if (hotelEntity != null)
                {
                    var hotel = _mapper.Map<Hotel, HotelViewModel>(hotelEntity);
                    return Ok(hotel);
                }
                else
                {
                    _message = $"Hotel o identyfikatorze {id} nie istnieje";
                    return NotFound(_message);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //POST: api/hotel
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddUpdateHotelViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newHotel = new Hotel();
                _mapper.Map(model, newHotel);
                _hotelRepository.AddEntity(newHotel);

                if (await _hotelRepository.SaveAsync())
                {
                    model.Id = newHotel.Id;
                    var newUri = Url.Link("GetHotel", new { id = model.Id });
                    return Created(newUri, model);
                }
                else
                {
                    _message = "Nie udało się dodać nowego hotelu";
                    return BadRequest(_message);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //PUT: api/hotel/id
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AddUpdateHotelViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var hotelEntity = await _hotelRepository.GetEntityById(id);
                if (hotelEntity == null)
                {
                    _message = $"Hotel o identyfikatorze {id} nie istnieje";
                    return NotFound(_message);
                }

                _mapper.Map(model, hotelEntity);
                _hotelRepository.UpdateEntity(hotelEntity);

                if (await _hotelRepository.SaveAsync())
                {
                    var updatedHotel = _mapper.Map<Hotel, AddUpdateHotelViewModel>(hotelEntity);
                    return Ok(updatedHotel);
                }
                else
                {
                    _message = "Nie udało się zaktualizować hotelu";
                    return BadRequest(_message);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //DELETE: api/hotel/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var hotelEntity = await _hotelRepository.GetEntityById(id);
                if (hotelEntity == null)
                {
                    _message = $"Hotel o identyfikatorze {id} nie istnieje";
                    return NotFound(_message);
                }

                _hotelRepository.DeactivateEntity(hotelEntity);

                if (await _hotelRepository.SaveAsync())
                {
                    _message = $"Usunięto hotel o identyfikatorze {id}";
                    return Ok(_message);
                }
                else
                {
                    _message = "Nie udało się usunąć hotelu";
                    return BadRequest(_message);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
