using System.ComponentModel;

namespace Flights_Application.Dtos
{
    public record FlightSearchParameters(
         
        [DefaultValue("12/25/2022 10:30:00 AM")]
         DateTime? FromDate,
        [DefaultValue("12/25/2022 10:30:00 AM")]
         DateTime? ToDate,
        [DefaultValue("Los Angeles")]
        string? From,
        [DefaultValue("Las Vegas")]
        string? Destination,
        [DefaultValue(1)]
        int? NumberOfPassengers
    );

}
