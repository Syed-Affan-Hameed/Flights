using Flights_Application.Data;
using Flights_Application.Domain.Errors;
using Flights_Application.Dtos;
using Flights_Application.ReadModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flights_Application.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {

        private Entities _entities;

        public BookingController(Entities entities)
        {
            _entities = entities;
        }

        [HttpGet("{email}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(IEnumerable<BookingRm>), 200)]
        public ActionResult<IEnumerable<BookingRm>> List([FromRoute]string email) {

            var bookings = _entities.Flights.ToArray()
               .SelectMany(f => f.Bookings
                   .Where(b => b.PassengerEmail == email)
                   .Select(b => new BookingRm(
                       f.Id,
                       f.Airline,
                       f.Price.ToString(),
                       new TimePlaceRm(f.Arrival.Place, f.Arrival.Time),
                       new TimePlaceRm(f.Departure.Place, f.Departure.Time),
                       b.NumberOfSeats,// number of seats booked by that email address
                       email
                       )));

            return Ok(bookings);
        }

        [HttpDelete]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public IActionResult CancelBooking(BookingDto dto)
        {
            var flight = _entities.Flights.Find(dto.FlightId);

            var error = flight.cancelBooking(dto.PassengerEmail, dto.NumberOfSeats);
            if (error is BookingNotFoundError)
            {
                return NotFound();
            }
            else if (error == null)// as we are recieving null from the cancel booking method if we have found the booking
            {
                _entities.SaveChanges();
                return NoContent();

            }
            else
            {

                throw new Exception($"There is something wrong in the cancel booking process:{error}");
            }

        }
    }
}
