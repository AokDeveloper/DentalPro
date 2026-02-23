import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../../../core/services/base.service';
import { Appointment } from '../../../core/models/appointment';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService extends BaseService<Appointment> {

  constructor(httpClient: HttpClient) {

    super(httpClient, 'appointments');
  }
}