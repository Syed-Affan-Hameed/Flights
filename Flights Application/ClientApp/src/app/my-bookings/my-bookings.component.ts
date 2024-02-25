import { Component, OnInit } from '@angular/core';
import { BookingRm,  } from '../api/models/booking-rm';
import { BookingDto} from '../api/models';
import { BookingService } from '../api/services';

import { AuthService } from '../auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-my-bookings',
  templateUrl: './my-bookings.component.html',
  styleUrls: ['./my-bookings.component.css']
})
export class MyBookingsComponent implements OnInit{


  bookings!: BookingRm[]
  constructor(private bookingService: BookingService, private authService: AuthService, private router: Router) {


  }

    ngOnInit(): void {



      this.bookingService.listBooking({ email: this.authService.currentUser?.email ?? '' }).subscribe(
        bookingsFromAPI => this.bookings = bookingsFromAPI, error => console.log("my-booking component error, error getting the booking lists filter by email",error)

      )


    }
  cancelBooking(booking: BookingRm) {
    console.log("Cancel Booking Method is Called");

    const dto: BookingDto = {
      flightId: booking.flightId,
      numberOfSeats: booking.numberofBookedSeats,
      passengerEmail: booking.passengerEmail
    }
    this.bookingService.cancelBookingBooking({ body: dto }).subscribe(

      _ => this.bookings= this.bookings.filter(b=>b!=booking),error=>console.log("cancel booking failed",error)

    )

  }

}
