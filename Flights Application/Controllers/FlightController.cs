using Flights_Application.Domain.Entities;
using Flights_Application.Domain.Errors;
using Flights_Application.Dtos;
using Flights_Application.ReadModels;
using Flights_Application.Data;

using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace Flights.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly ILogger<FlightController> _logger;
        private readonly Entities _entities;
        public FlightController(ILogger<FlightController> logger, Entities entities)
        {
            _entities = entities;
            _logger = logger;
        }
        static Random random = new Random();




        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(FlightRm[]), 200)]
        public ActionResult<FlightRm[]> Search([FromQuery] FlightSearchParameters flightSearchParameters)
        {
            _logger.LogInformation("Reading flight search parameters{flightSearchParameters}", flightSearchParameters);


            IQueryable<Flight> filteredFlights = _entities.Flights;

            if (!string.IsNullOrWhiteSpace(flightSearchParameters.Destination))

                filteredFlights = filteredFlights.Where(f => f.Arrival.Place.Contains(flightSearchParameters.Destination));

            if (!string.IsNullOrWhiteSpace(flightSearchParameters.From))

                filteredFlights = filteredFlights.Where(f => f.Departure.Place.Contains(flightSearchParameters.From));

            if (flightSearchParameters.FromDate != null)

                filteredFlights = filteredFlights.Where(f => f.Departure.Time >= flightSearchParameters.FromDate.Value.Date);

            if (flightSearchParameters.ToDate != null)

                filteredFlights = filteredFlights.Where(f => f.Departure.Time >= flightSearchParameters.ToDate.Value.Date.AddDays(1));

            if (flightSearchParameters.NumberOfPassengers != 0 && flightSearchParameters.NumberOfPassengers != null)

                filteredFlights = filteredFlights.Where(f => f.RemainingNumberOfSeats >= flightSearchParameters.NumberOfPassengers);

            // Do this instead of for looping
            var flightRmList = _entities.Flights.Select(flight => new FlightRm(
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
        [ProducesResponseType(typeof(FlightRm), 200)]
        [HttpGet("{id:Guid}")]

        public ActionResult<FlightRm> Find([FromRoute] Guid id)
        {


            // iterates over the array and finds us our unique flight
            var flight = _entities.Flights.SingleOrDefault(f => f.Id == id);

            if (flight == null)
            {
                return NotFound();
            }


            var FlightReadModel = new FlightRm(
                 flight.Id,
                 flight.Airline,
                 flight.Price,
                 new TimePlaceRm(flight.Departure.Place, flight.Departure.Time),
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
            System.Diagnostics.Debug.WriteLine("The Booking information has reached!" + dto.FlightId);

            // flightFound is boolean true if found, false if not and flights is now array of Flight Entity
            //var flightFound = flights.Any(f => f.Id == dto.FlightId); this statement returns a boolean corresponding to the  presence of the flight with matching flightid
            var flightFound = _entities.Flights.SingleOrDefault(f => f.Id == dto.FlightId); // this statements returns athe whole flight entity instance with matching flightId


            if (flightFound == null)
            {
                return NotFound(dto.FlightId);
            }

            var errorCheckObject = flightFound.makeBooking(dto.PassengerEmail, dto.NumberOfSeats);


            if (errorCheckObject is OverbookError)
            {
                return Conflict(new { message = " Requested number of seats exceeds the remaining available(" + flightFound.RemainingNumberOfSeats + ") seats for the flight " });

            }

            try
            {
                _entities.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(new { message = " Probable Race Condition Detected!" });

            }



            return CreatedAtAction(nameof(Find), new { id = dto.FlightId });


        }





    }
}