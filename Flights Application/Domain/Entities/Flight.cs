using Flights_Application.Domain.Errors;

namespace Flights_Application.Domain.Entities
{
    public class Flight
    {

        public Guid Id { get; set; }
        public string Airline { get; set; }
        public string Price { get; set; }
        public TimePlace Departure { get; set; }
        public TimePlace Arrival { get; set; }
        public int RemainingNumberOfSeats { get; set; }

        public IList<Booking> Bookings = new List<Booking>();
       

        public Flight()
        {

        }

        public Flight(
            Guid id,
            string airline,
            string price,
            TimePlace departure,
            TimePlace arrival,
            int remainingNumberOfSeats
        )
        {
            Id = id;
            Airline = airline;
            Price = price;
            Departure = departure;
            Arrival = arrival;
            RemainingNumberOfSeats = remainingNumberOfSeats;
        }
        internal object? makeBooking(string passengerEmail, byte numberOfSeats)
        {
            var flightFound = this;

            if (flightFound.RemainingNumberOfSeats < numberOfSeats)
            {
                // making new error for this scenario
                //return Conflict(new { message = " Requested number of seats exceeds the remaining available(" + flightFound.RemainingNumberOfSeats + ") seats for the flight " });
                
                return new OverbookError();// We are returning OverBook object, it is an empty class but just to identify the error in th controller method 
            }

            var BookingEntity = new Booking(
               passengerEmail,
               numberOfSeats
                );


            flightFound.RemainingNumberOfSeats -= numberOfSeats;
            flightFound.Bookings.Add(BookingEntity); // Using 'Booking' to write to our database, every flight has its own set of bookings
            return null;
        }
        internal object? cancelBooking(string passengerEmail, byte numberOfSeats)
        {
            var booking = Bookings.FirstOrDefault(b=>passengerEmail== b.PassengerEmail && numberOfSeats==b.NumberOfSeats);

            if (booking == null)
            {
                return new BookingNotFoundError();
            }

            Bookings.Remove(booking);
            RemainingNumberOfSeats += numberOfSeats;
            return null;
        }


    }

}
