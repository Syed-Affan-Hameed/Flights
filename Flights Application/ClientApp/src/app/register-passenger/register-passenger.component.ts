import { Component, OnInit } from '@angular/core';
import { PassengerService } from '../api/services';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthService, User } from '../auth/auth.service';
import { Router } from '@angular/router';
import { FindFlight$Params } from '../api/fn/flight/find-flight';
import { FindPassenger$Params } from '../api/fn/passenger/find-passenger';

@Component({
  selector: 'app-register-passenger',
  templateUrl: './register-passenger.component.html',
  styleUrls: ['./register-passenger.component.css']
})
export class RegisterPassengerComponent implements OnInit {

  constructor(private passengerService: PassengerService, private formbuilder: FormBuilder, private authService: AuthService, private router:Router) { }

  PassengerRegistrationForm = this.formbuilder.group({
    email: ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(100)])],
    firstName: ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(35)])],
    lastName: ['', Validators.compose([Validators.required, Validators.minLength(3), Validators.maxLength(35)])],
    isFemale: [true, Validators.required]
  })

  ngOnInit(): void {
   // throw new Error('Method not implemented.');
  }
  checkPassenger(): void {

    const emailObjectForCheckingifUserExists: FindPassenger$Params  = { email: this.PassengerRegistrationForm.get('email')?.value?.toString() };
    this.passengerService.findPassenger(emailObjectForCheckingifUserExists).subscribe(p => {
      console.log("passenger Exists logging in and redirecting to searchflights");
      this.logIn();
    }, error => {
      if (error.status != 404) {
        console.log(error)
      }
       
    })
    this.passengerService.findPassenger( emailObjectForCheckingifUserExists)
  }


  register() {
    console.log("Form values for registration", this.PassengerRegistrationForm.value);
    // this is a post method so have to populate the body.
   // let serviceObjectEmailOnly: User = { email: this.PassengerRegistrationForm.get('email')?.value }
    this.passengerService.registerPassenger({ body: this.PassengerRegistrationForm.value }).subscribe(p =>
    this.logIn(), error => console.log(error))

  }

  private logIn = () => {
    this.authService.login({ email: this.PassengerRegistrationForm.get('email')?.value });
    this.router.navigate(['/search-flights']);
  }

}
