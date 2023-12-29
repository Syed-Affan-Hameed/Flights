

namespace Flights_Application.Domain.Entities
{
    public record Booking(


      Guid FlightId,
      string PassengerEmail,
      byte NumberOfSeats
        );
}
