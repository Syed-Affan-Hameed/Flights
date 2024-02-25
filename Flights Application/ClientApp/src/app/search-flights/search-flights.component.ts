import { Component, OnInit } from '@angular/core';
import { FlightService } from '../api/services';
import { FlightRm, TimePlaceRm } from '../api/models';
import { FormBuilder} from '@angular/forms';


@Component({
  selector: 'app-search-flights',
  templateUrl: './search-flights.component.html',
  styleUrls: ['./search-flights.component.css']
})
export class SearchFlightsComponent implements OnInit {

  searchResult: FlightRm[] = [];
  constructor(private flightService: FlightService, private formBuilder: FormBuilder) {

  }
  searchForm = this.formBuilder.group({

    from: [''],
    destination: [''],
    fromDate: [''],
    toDate: [''],
    numberOfPassengers: [1],
  })
    ngOnInit(): void {
      this.search();
    }
  search() {
    this.flightService.searchFlight(this.searchForm.value).subscribe(response => this.searchResult = response, this.handleError)
  }
  private handleError = (err: any) => {

 

    console.log("Response Error. Status: ", err.status)
    console.log("Response Error. Status Text: ", err.statusText)
    console.log(err)
  }
}

