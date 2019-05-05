using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using HotelReservation.Data.Entities;
using HotelReservation.Data.ViewModels.CustomerViewModels;
using System.Linq;
using HotelReservation.Data.Repositories.Interfaces;

namespace HotelReservation.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : BaseController
    {
        private ICustomerRepository _customerRepository;
        private string _message;

        public CustomerController(ICustomerRepository customerRepository, IMapper mapper) : base(mapper)
        {
            _customerRepository = customerRepository;
        }

        // GET: api/customer
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var customerEntities = await _customerRepository.GetAll();
                if (customerEntities.Any())
                {
                    var customers = _mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerViewModel>>(customerEntities);
                    return Ok(customers);
                }
                else
                {
                    _message = "Baza danych klientów jest pusta";
                    return BadRequest(_message);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/customer/id
        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var customerEntity = await _customerRepository.GetEntityById(id);
                if (customerEntity != null)
                {
                    var customer = _mapper.Map<Customer, CustomerViewModel>(customerEntity);
                    return Ok(customer);
                }
                else
                {
                    _message = $"Klient o identyfikatorze {id} nie istnieje";
                    return NotFound(_message);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //POST: api/customer
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddUpdateCustomerViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newCustomer = new Customer();
                _mapper.Map(model, newCustomer);
                _customerRepository.AddEntity(newCustomer);

                if (await _customerRepository.SaveAsync())
                {
                    model.Id = newCustomer.Id;
                    var newUri = Url.Link("GetCustomer", new { id = model.Id });
                    return Created(newUri, model);
                }
                else
                {
                    _message = "Nie udało się dodać nowego klienta";
                    return BadRequest(_message);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //PUT: api/customer/id
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AddUpdateCustomerViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var customerEntity = await _customerRepository.GetEntityById(id);
                if (customerEntity == null)
                {
                    _message = $"Klient o identyfikatorze {id} nie istnieje";
                    return NotFound(_message);
                }

                _mapper.Map(model, customerEntity);
                _customerRepository.UpdateEntity(customerEntity);

                if (await _customerRepository.SaveAsync())
                {
                    var updatedHotel = _mapper.Map<Customer, AddUpdateCustomerViewModel>(customerEntity);
                    return Ok(updatedHotel);
                }
                else
                {
                    _message = "Nie udało się zaktualizować danych klienta";
                    return BadRequest(_message);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //DELETE: api/customer/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var customerEntity = await _customerRepository.GetEntityById(id);
                if (customerEntity == null)
                {
                    _message = $"Klient o identyfikatorze {id} nie istnieje";
                    return NotFound(_message);
                }

                _customerRepository.DeactivateEntity(customerEntity);

                if (await _customerRepository.SaveAsync())
                {
                    _message = $"Usunięto klienta o identyfikatorze {id}";
                    return Ok(_message);
                }
                else
                {
                    _message = "Nie udało się usunąć klienta";
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
