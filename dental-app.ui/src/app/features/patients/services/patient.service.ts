import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../../../core/services/base.service';
import { PatientList } from '../../../core/models/patients/patientList';
import { PatientCreate } from '../../../core/models/patients/patientCreate';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class PatientService extends BaseService<PatientList> {

  constructor(httpClient: HttpClient) {
    // apiUrl = environment.apiUrl + 'patients' şeklinde set edildi
    super(httpClient, 'patients');
  }

  createPatient(payload: PatientCreate): Observable<any> {
    return this.httpClient.post(`${this.apiUrl}`, payload);
  }

  // 🌟 YENİ EKLENEN METOT: Sadece hastalara özel geçmiş randevu listesi
  // this.apiUrl zaten '/api/patients' olduğu için devamına ID ve endpoint'i ekliyoruz.
  getCompletedAppointments(patientId: string | number): Observable<any> {
    return this.httpClient.get<any>(`${this.apiUrl}/${patientId}/completed-appointments`);
  }

  getGroupedCategories(): Observable<any[]> {
    // this.apiUrl zaten '/api/patients' demek. Sonuna sadece alt rotayı ekliyoruz.
    return this.httpClient.get<any[]>(`${this.apiUrl}/categories/grouped`);
  }
  getPatientDetail(patientId: string) {
        return this.httpClient.get(`${this.apiUrl}/${patientId}/detail`);
  }
  }
  
