using Flights_Application.Domain.Entities;
using Flights_Application.Dtos;
using Flights_Application.ReadModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flights_Application.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private static IList<Passenger> Passengers = new List<Passenger>();
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
            Passengers.Add(passengerEntity); //Using entity becasue we are writing to our database
            System.Diagnostics.Debug.WriteLine(Passengers.Count);
            return CreatedAtAction(nameof(Find),new {email=newPassengerDto.Email});
        }
        [HttpGet("{email}")]
        [ProducesResponseType(200)]
        public ActionResult<PassengerRm> Find([FromRoute] string email)
        {
            var passenger= Passengers.FirstOrDefault(p => p.Email == email);

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
