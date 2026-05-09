import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../../../core/services/base.service';
import { Appointment } from '../../../core/models/appointment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService extends BaseService<Appointment> {

  constructor(httpClient: HttpClient) {
    super(httpClient, 'appointments');
  }

  // 🌟 YENİ: Artık payload bekliyor
  cancelAppointment(id: string, payload: any): Observable<any> {
    return this.httpClient.put(`${this.apiUrl}/${id}/cancel`, payload);
  }

  // 🌟 YENİ: Artık payload bekliyor
  completeAppointment(id: string, payload: any): Observable<any> {
    return this.httpClient.put(`${this.apiUrl}/${id}/complete`, payload);
  }

    didntcomeAppointment(id: string, payload: any): Observable<any> {
    return this.httpClient.put(`${this.apiUrl}/${id}/didntcome`, payload);
  }
}