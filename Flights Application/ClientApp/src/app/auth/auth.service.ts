import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() { }
   
  currentUser?: User;

  login(user: User) {
    console.log("Logging the user with email", user.email);
    this.currentUser = user;

  }

}

export interface User {

  email: string | null| undefined;
  
}
