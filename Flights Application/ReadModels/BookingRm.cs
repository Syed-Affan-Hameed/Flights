namespace Flights_Application.ReadModels
{
    public record BookingRm(
        Guid FlightId,
        string Airline,
        string price,
        TimePlaceRm Arrival,
        TimePlaceRm Departure,
        int NumberofBookedSeats,
        string PassengerEmail
        );
   
}
