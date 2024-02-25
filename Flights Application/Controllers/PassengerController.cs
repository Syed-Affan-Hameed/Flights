using Flights_Application.Domain.Entities;
using Flights_Application.Dtos;
using Flights_Application.ReadModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Flights_Application.Data;

namespace Flights_Application.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly Entities _entities;

        // We are using .NET dependency injection to inject in the constructor and use it in the controller.
        public PassengerController(Entities entities)
        {
          _entities = entities;
        }
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult Register(NewPassengerDto newPassengerDto)
        {
            var passengerEntity = new Passenger(
                newPassengerDto.Email,
                newPassengerDto.FirstName,
                newPassengerDto.LastName,
                newPassengerDto.Gender);
            _entities.Passengers.Add(passengerEntity); //Using entity becasue we are writing to our database
                                                       //System.Diagnostics.Debug.WriteLine(_entities.Passengers.Count);

            _entities.SaveChanges();
            return CreatedAtAction(nameof(Find),new {email=newPassengerDto.Email});
        }
        [HttpGet("{email}")]
        [ProducesResponseType(200)]
        public ActionResult<PassengerRm> Find([FromRoute] string email)
        {
            var passenger= _entities.Passengers.FirstOrDefault(p => p.Email == email);

            if(passenger == null)
            {
                return NotFound();
            }

            var passengerRm = new PassengerRm(
                passenger.Email,
                passenger.FirstName,
                passenger.LastName,
                passenger.Gender
                );

            return Ok(passengerRm);

        }
    }
}
