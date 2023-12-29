import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FlightService } from '../api/services';
import { BookingDto, FlightRm } from '../api/models';
import { AuthService } from '../auth/auth.service';
import { FormBuilder,Validators } from '@angular/forms';

@Component({
  selector: 'app-book-flight',
  templateUrl: './book-flight.component.html',
  styleUrls: ['./book-flight.component.css']
})
export class BookFlightComponent implements OnInit {

  constructor(private formbuilder :FormBuilder, private route: ActivatedRoute, private flightService: FlightService, private router: Router, private authService: AuthService) { }

  flightId: string = 'not loaded'
  flightDetailsFromAPIResponse!: FlightRm;

  BookFlightForm = this.formbuilder.group({

    number: [1, Validators.compose([Validators.required,Validators.min(1),Validators.max(254)])]
  }
)

  ngOnInit(): void {

    if (this.authService.currentUser == undefined) {

      this.router.navigate(['/register-passenger'])
    }

    this.route.paramMap
      .subscribe(p => this.findFlightForUI(p.get("flightId")))
  }

  private findFlightForUI = (flightId: string | null) => {
    this.flightId = flightId ?? 'not passed';

    this.flightService.findFlight({ id: this.flightId }).subscribe(
      flight => this.flightDetailsFromAPIResponse = flight, this.handleError)


  }
  private handleError = (err: any) => {

    if (err.status == 404) {
      alert("Flight not found!")
      this.router.navigate(['/search-flights'])
    }

    console.log("Response Error. Status: ", err.status)
    console.log("Response Error. Status Text: ", err.statusText)
    console.log(err)
  }
  Book() {

    console.log(`Booking ${this.BookFlightForm.get('number')?.value} passengers for the flight ${this.flightDetailsFromAPIResponse.id} `)

    if (this.BookFlightForm.invalid) {

      return;
    }

    const BookingDetails: BookingDto = {
      flightId: this.flightDetailsFromAPIResponse.id,
      passengerEmail: this.authService.currentUser?.email,
      numberOfSeats: this.BookFlightForm.get('number')?.value

    }

    this.flightService.bookFlight({ body: BookingDetails })
      .subscribe(b => { this.router.navigate(['my-bookings']); console.log("succesfully booked!") }, error =>this.handleError(error));
  }

  get numberOfSeats() {

    return this.BookFlightForm.controls.number;
  }

}
