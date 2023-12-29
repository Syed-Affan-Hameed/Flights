using Flights_Application.Domain.Entities;
using Flights_Application.Dtos;
using Flights_Application.ReadModels;

using Microsoft.AspNetCore.Mvc;

namespace Flights.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly ILogger<FlightController> _logger;

        public FlightController(ILogger<FlightController> logger)
        {
            _logger = logger;
        }
        static Random random = new Random();
       
  
        static private Flight[] flights = new Flight[]

            {
        new (   Guid.NewGuid(),
                "American Airlines",
                random.Next(90, 5000).ToString(),
                new TimePlace("Zurich",DateTime.Now.AddHours(random.Next(1, 23))),
                new TimePlace("Baku",DateTime.Now.AddHours(random.Next(4, 25))),
                    random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "Adria Airways",
                random.Next(90, 5000).ToString(),
                new TimePlace("Ljubljana",DateTime.Now.AddHours(random.Next(1, 15))),
                new     ("Warsaw",DateTime.Now.AddHours(random.Next(4, 19))),
                    random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "ABA Air",
                random.Next(90, 5000).ToString(),
                new TimePlace("Praha Ruzyne",DateTime.Now.AddHours(random.Next(1, 55))),
                new TimePlace("Paris",DateTime.Now.AddHours(random.Next(4, 58))),
                    random.Next(1, 853)),
        new (   Guid.NewGuid(),
                "AB Corporate Aviation",
                random.Next(90, 5000).ToString(),
                new TimePlace("Le Bourget",DateTime.Now.AddHours(random.Next(1, 58))),
                new TimePlace("Zagreb",DateTime.Now.AddHours(random.Next(4, 60))),
                    random.Next(1, 853))
            };

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(FlightRm[]), 200)]
        public ActionResult<FlightRm[]> Search()
        {
            // Do this instead of for looping
            var flightRmList = flights.Select(flight => new FlightRm(
                 flight.Id,
                 flight.Airline,
                 flight.Price,
                 new TimePlaceRm(flight.Departure.Place, flight.Departure.Time),
                 new TimePlaceRm(flight.Departure.Place, flight.Departure.Time),
                 flight.RemainingNumberOfSeats
                 )).ToArray();


            return flightRmList;
        }

        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(FlightRm),200)]
        [HttpGet("{id:Guid}")]

        public ActionResult<FlightRm> Find([FromRoute] Guid id)
        {
           // iterates over the array and finds us our unique flight
            var flight= flights.SingleOrDefault(f => f.Id == id);

            if(flight == null)
            {
                return NotFound();  
            }
            

            var FlightReadModel = new FlightRm(
                 flight.Id,
                 flight.Airline,
                 flight.Price,
                 new TimePlaceRm(flight.Departure.Place,flight.Departure.Time),
                 new TimePlaceRm(flight.Departure.Place, flight.Departure.Time),
                 flight.RemainingNumberOfSeats
                 );


            return Ok(FlightReadModel);
        }
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesResponseType(201)]
        public IActionResult Book(BookingDto dto)
        {
            System.Diagnostics.Debug.WriteLine("The Booking information has reached!"+dto.FlightId);

            // flightFound is boolean true if found, false if not
            //var flightFound = flights.Any(f => f.Id == dto.FlightId); this statement returns a boolean corresponding to the  presence of the flight with matching flightid
            var flightFound = flights.SingleOrDefault(f => f.Id == dto.FlightId); // this statements returns athe whole flight entity instance with matching flightId
            if (flightFound==null)
            {
                return NotFound(dto.FlightId);
            }
            if(flightFound.RemainingNumberOfSeats< dto.NumberOfSeats)
            {
                return Conflict( new {message=" Requested number of seats exceeds the remaining available("+ flightFound.RemainingNumberOfSeats + ") seats for the flight "  });
            }

            var BookingEntity = new Booking(
                dto.FlightId,
                dto.PassengerEmail,
                dto.NumberOfSeats
                );

            flightFound.RemainingNumberOfSeats -= dto.NumberOfSeats;
            flightFound.Bookings.Add(BookingEntity); // Using 'Booking' to write to our database, every flight has its own set of bookings
            return CreatedAtAction(nameof(Find), new { id = dto.FlightId });
      

        }
      
        

    }
}