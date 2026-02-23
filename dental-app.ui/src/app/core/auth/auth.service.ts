import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // Sizin Backend'in çalıştığı doğru port numarası
  private apiUrl = 'https://localhost:7025/api/auth/login'; 

  constructor(private http: HttpClient) { }

  login(credentials: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, credentials).pipe(
      tap(response => {
        // 1. Backend'den token başarıyla geldiyse
        if (response && response.token) {
          
          // Token'ı tarayıcı hafızasına kaydet
          localStorage.setItem('token', response.token);
          
          // 2. JWT (JSON Web Token) Çözümleme İşlemi
          try {
            // Token 3 parçadan oluşur (Header.Payload.Signature). Bize ortadaki Payload lazım.
            const payloadBase64 = response.token.split('.')[1]; 
            const decodedJson = atob(payloadBase64); // Base64 şifresini çöz
            const decodedToken = JSON.parse(decodedJson); // JSON formatına çevir
            
            // Geliştirici olarak token'ın içinde ne var ne yok görelim (F12 Console'da yazar)
            console.log("Token İçeriği:", decodedToken);

            // 3. C# tarafında eklediğimiz 'DoctorId' Claim'ini okuyup kaydedelim
            if (decodedToken.DoctorId) {
              localStorage.setItem('doctorId', decodedToken.DoctorId);
              console.log("Sisteme giriş yapan Doktor ID'si kaydedildi:", decodedToken.DoctorId);
            } else {
              console.warn("DİKKAT: Backend'den dönen Token içinde 'DoctorId' bulunamadı! Veritabanındaki AppUserId eşleşmesini kontrol edin.");
            }
            
          } catch (error) {
            console.error('Token çözümlenirken kritik bir hata oluştu:', error);
          }
        }
      })
    );
  }

  logout() {
    // Çıkış yapıldığında hafızayı tamamen temizle
    localStorage.removeItem('token');
    localStorage.removeItem('doctorId');
  }

  getCurrentDoctorId(): string | null {
    // Randevu kaydederken veya sorgularken bu metodu çağıracağız
    return localStorage.getItem('doctorId');
  }

  isAuthenticated(): boolean {
    // İçeride bir token varsa true, yoksa false döner (AuthGuard için)
    return !!localStorage.getItem('token');
  }
}