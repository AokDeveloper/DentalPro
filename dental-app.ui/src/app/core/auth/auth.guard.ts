import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './auth.service';

// Angular 15+ ile gelen yeni nesil, fonksiyonel Guard yapısı
export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Eğer kullanıcı giriş yapmışsa (token varsa) içeri al
  if (authService.isAuthenticated()) {
    return true; 
  }

  // Giriş yapmamışsa Login sayfasına yönlendir ve kapıyı kapat
  router.navigate(['/login']);
  return false;
};