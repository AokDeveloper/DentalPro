import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../../../core/services/base.service';
import { PatientList } from '../../../core/models/patients/patientList';
import { PatientCreate } from '../../../core/models/patients/patientCreate';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
// "export" kelimesine DİKKAT EDİN. Bu olmazsa diğer dosyalar burayı göremez.
export class PatientService extends BaseService<PatientList> {

  constructor(httpClient: HttpClient) {
    super(httpClient, 'patients');
  }
createPatient(payload: PatientCreate): Observable<any> {
    return this.httpClient.post(`${this.apiUrl}`, payload);
  }
}