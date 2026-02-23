import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../../../core/services/base.service';
import { Patient } from '../../../core/models/patient';

@Injectable({
  providedIn: 'root'
})
// "export" kelimesine DİKKAT EDİN. Bu olmazsa diğer dosyalar burayı göremez.
export class PatientService extends BaseService<Patient> {

  constructor(httpClient: HttpClient) {
    super(httpClient, 'patients');
  }
}