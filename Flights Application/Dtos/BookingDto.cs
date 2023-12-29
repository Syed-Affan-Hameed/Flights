using System.ComponentModel.DataAnnotations;

namespace Flights_Application.Dtos
{
    public record BookingDto(


      [Required]  Guid FlightId,
      [Required][EmailAddress][StringLength(100,MinimumLength =3)] string PassengerEmail,
      [Required] [Range(1,254 )]byte NumberOfSeats
        );
}
