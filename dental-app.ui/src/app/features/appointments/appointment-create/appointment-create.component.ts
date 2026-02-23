import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

// PrimeNG Görsel Modülleri
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { CardModule } from 'primeng/card';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

// Servisler ve Veri Modelleri
import { AppointmentService } from '../services/appointment.service';
import { PatientService } from '../../patients/services/patient.service';
import { Patient } from '../../../core/models/patient';
import { AuthService } from '../../../core/auth/auth.service';

@Component({
  selector: 'app-appointment-create',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule, 
    RouterModule,
    DropdownModule, 
    ButtonModule, 
    CalendarModule, 
    InputTextareaModule, 
    CardModule, 
    ToastModule
  ],
  providers: [MessageService], // Bildirimler için zorunlu
  templateUrl: './appointment-create.component.html'
})
export class AppointmentCreateComponent implements OnInit {

  // Backend'e gönderilecek Randevu Verisi
  appointmentModel: any = {
    patientId: null,
    date: null,
    notes: '',
    
  };

  patients: Patient[] = []; // Dropdown içine dolacak hastalar
  loading: boolean = false; // Butonun dönme animasyonu için

  constructor(
    private appointmentService: AppointmentService,
    private patientService: PatientService,
    private messageService: MessageService,
    private router: Router,
    private authService: AuthService
  ) {}

  // Sayfa açıldığı anda çalışır
  ngOnInit() {
    this.loadPatients();
  }

  // Veritabanındaki hastaları getirir
  loadPatients() {
    this.patientService.getList().subscribe({
      next: (response: any) => {
        // Backend'den gelen veri yapısına göre (patients veya data) listeyi dolduruyoruz
        this.patients = response.patients || response.data || [];
      },
      error: (err) => console.error('Hastalar yüklenemedi:', err)
    });
  }

  // Kaydet Butonuna tıklandığında çalışır
  save() {
    // 1. Form Doğrulaması
    if (!this.appointmentModel.patientId || !this.appointmentModel.date) {
      this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Lütfen hasta ve tarih alanlarını doldurunuz.' });
      return;
    }

    // 2. DİNAMİK DOKTOR ID'SİNİ ALMA
    const currentDoctorId = this.authService.getCurrentDoctorId();
    
    // Güvenlik: Eğer ID yoksa (localStorage silindiyse vs.) işlemi durdur
    if (!currentDoctorId) {
      this.messageService.add({ severity: 'error', summary: 'Yetki Hatası', detail: 'Oturum bilginiz bulunamadı. Lütfen tekrar giriş yapın.' });
      return;
    }

    // 3. Backend'e gidecek nihai paketi hazırlama
    const payload = {
      ...this.appointmentModel, // Formdan gelen verileri kopyala
      doctorId: currentDoctorId // Hafızadan aldığımız Doktor ID'sini ekle
    };

    this.loading = true; 
    
    // 4. İsteği Gönderme (Artık 'payload' değişkenini gönderiyoruz)
    this.appointmentService.add(payload).subscribe({
      next: (res) => {
        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Randevu başarıyla oluşturuldu.' });
        
        setTimeout(() => {
          this.router.navigate(['/appointments']);
        }, 1500);
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Kayıt Hatası', detail: 'İşlem sırasında bir hata oluştu.' });
        this.loading = false;
      }
  
    });
  }
}