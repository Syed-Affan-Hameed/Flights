/* tslint:disable */
/* eslint-disable */
import { HttpClient, HttpContext } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';

import { BookingRm } from '../models/booking-rm';
import { cancelBookingBooking } from '../fn/booking/cancel-booking-booking';
import { CancelBookingBooking$Params } from '../fn/booking/cancel-booking-booking';
import { listBooking } from '../fn/booking/list-booking';
import { ListBooking$Params } from '../fn/booking/list-booking';
import { listBooking$Plain } from '../fn/booking/list-booking-plain';
import { ListBooking$Plain$Params } from '../fn/booking/list-booking-plain';

@Injectable({ providedIn: 'root' })
export class BookingService extends BaseService {
  constructor(config: ApiConfiguration, http: HttpClient) {
    super(config, http);
  }

  /** Path part for operation `listBooking()` */
  static readonly ListBookingPath = '/Booking/{email}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `listBooking$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  listBooking$Plain$Response(params: ListBooking$Plain$Params, context?: HttpContext): Observable<StrictHttpResponse<Array<BookingRm>>> {
    return listBooking$Plain(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `listBooking$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  listBooking$Plain(params: ListBooking$Plain$Params, context?: HttpContext): Observable<Array<BookingRm>> {
    return this.listBooking$Plain$Response(params, context).pipe(
      map((r: StrictHttpResponse<Array<BookingRm>>): Array<BookingRm> => r.body)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `listBooking()` instead.
   *
   * This method doesn't expect any request body.
   */
  listBooking$Response(params: ListBooking$Params, context?: HttpContext): Observable<StrictHttpResponse<Array<BookingRm>>> {
    return listBooking(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `listBooking$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  listBooking(params: ListBooking$Params, context?: HttpContext): Observable<Array<BookingRm>> {
    return this.listBooking$Response(params, context).pipe(
      map((r: StrictHttpResponse<Array<BookingRm>>): Array<BookingRm> => r.body)
    );
  }

  /** Path part for operation `cancelBookingBooking()` */
  static readonly CancelBookingBookingPath = '/Booking';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `cancelBookingBooking()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  cancelBookingBooking$Response(params?: CancelBookingBooking$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return cancelBookingBooking(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `cancelBookingBooking$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  cancelBookingBooking(params?: CancelBookingBooking$Params, context?: HttpContext): Observable<void> {
    return this.cancelBookingBooking$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

}
