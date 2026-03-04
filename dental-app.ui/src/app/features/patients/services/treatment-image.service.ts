import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

// Kendi projenizdeki BaseService yolunu doğru ayarladığınızdan emin olun
import { environment } from '../../../../environments/environment';
import { BaseService } from '../../../core/services/base.service'; 

// 1. Angular'ın bu servisi tanıması için gereken DAMGA (Bu eksikse NG2003 hatası alırsınız)
@Injectable({
  providedIn: 'root'
})
// 2. Dışarıdan erişilebilmesi için gereken EXPORT kelimesi (Bu eksikse TS2306 hatası alırsınız)
export class TreatmentImageService extends BaseService<any> {

  constructor(protected override httpClient: HttpClient) {
    // Base servise API'nin son kısmını gönderiyoruz
    super(httpClient, 'TreatmentImages'); 
  }

  // Hastaya Özel Fotoğrafları Getiren Metot
 getPatientImages(patientId: string): Observable<any> {
    // apiUrl'in sonunda '/' varsa onu sil, yoksa aynen bırak ve birleştir
    const baseUrl = environment.apiUrl.endsWith('/') ? environment.apiUrl.slice(0, -1) : environment.apiUrl;
    const url = `${baseUrl}/patients/${patientId}/images`;
    
    return this.httpClient.get<any>(url);
  }

  // Fotoğraf Yükleme Metodu (İleride kullanacağız)
  uploadPatientImage(formData: FormData): Observable<any> {
    const baseUrl = environment.apiUrl.endsWith('/') ? environment.apiUrl.slice(0, -1) : environment.apiUrl;
    const url = `${baseUrl}/patients/images`; 
    
    return this.httpClient.post<any>(url, formData);
  }
}