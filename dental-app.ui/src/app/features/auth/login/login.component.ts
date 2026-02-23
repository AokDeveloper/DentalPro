import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

// PrimeNG Modülleri
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

import { AuthService } from '../../../core/auth/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule, 
    InputTextModule, 
    PasswordModule, 
    ButtonModule, 
    CardModule, 
    ToastModule
  ],
  providers: [MessageService],
  templateUrl: './login.component.html'
})
export class LoginComponent {
  
  // Backend'in beklediği giriş modeli (email mi username mi olduğunu backendinize göre değiştirin)
  loginModel = {
    email: '',
    password: ''
  };

  loading: boolean = false;

  constructor(
    private authService: AuthService,
    private messageService: MessageService,
    private router: Router
  ) {}

  onLogin() {
    if (!this.loginModel.email || !this.loginModel.password) {
      this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Lütfen tüm alanları doldurun.' });
      return;
    }

    this.loading = true;

    this.authService.login(this.loginModel).subscribe({
      next: (res) => {
        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Giriş yapıldı, yönlendiriliyorsunuz...' });
        
        // Başarılı girişte ana sayfaya (veya dashboarda) yönlendir
        setTimeout(() => {
          this.router.navigate(['/']); 
        }, 1000);
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Giriş Başarısız', detail: 'E-posta veya şifre hatalı.' });
        this.loading = false;
      }
    });
  }
}