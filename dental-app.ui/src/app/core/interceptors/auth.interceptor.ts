import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  
  // 1. İstisna: Login aşamasında token gerekmediği için direkt pas geçiyoruz
  if (req.url.includes('/api/auth/login')) {
      return next(req);
  }

  // 2. Token'ı hafızadan alıyoruz
  const token = localStorage.getItem('token');

  // 3. Token varsa isteği klonlayıp Authorization başlığını ekliyoruz
  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  // Router'ı fonksiyonel yapıya uygun şekilde inject ediyoruz
  const router = inject(Router);

  // 4. İsteği yola çıkarıyor ve olası sunucu hatalarını (401) dinliyoruz
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      // Eğer backend "Yetkisiz Erişim" (Süresi dolmuş token) derse:
      if (error.status === 401) {
        localStorage.removeItem('token');
        localStorage.removeItem('doctorId'); // Varsa diğer verileri de temizle
        router.navigate(['/login']); // Kullanıcıyı giriş ekranına yönlendir
      }
      
      // Hatayı uygulamanın geri kalanına fırlat ki ekranda (Toast vb.) gösterilebilsin
      return throwError(() => error);
    })
  );
};