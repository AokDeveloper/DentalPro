import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '../../../core/services/base.service';
import { Supervisor } from '../../../core/models/supervisor';


@Injectable({
  providedIn: 'root'
})
// "export" kelimesine DİKKAT EDİN. Bu olmazsa diğer dosyalar burayı göremez.
export class SupervisorService extends BaseService<Supervisor> {

  constructor(httpClient: HttpClient) {
    super(httpClient, 'supervisors');
  }
}