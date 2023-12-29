using System.ComponentModel.DataAnnotations;

namespace Flights_Application.ReadModels
{
    public record PassengerRm(
        string Email,
        string FirstName,
        string LastName,
        bool Gender
);
}
