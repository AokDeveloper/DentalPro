import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { DropdownModule } from 'primeng/dropdown';
import { CalendarModule } from 'primeng/calendar';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { MessageService, PrimeNGConfig } from 'primeng/api';
import { InputSwitchModule } from 'primeng/inputswitch';
import { AppointmentService } from '../services/appointment.service';
import { PatientService } from '../../patients/services/patient.service';
import { AuthService } from '../../../core/auth/auth.service';

@Component({
  selector: 'app-appointment-create',
  standalone: true,
  imports: [CommonModule, FormsModule, DropdownModule, CalendarModule, InputTextareaModule, ButtonModule, ToastModule,InputSwitchModule],
  providers: [MessageService],
  templateUrl: './appointment-create.component.html'
})
export class AppointmentCreateComponent implements OnInit {

  appointmentModel: any = {
    patientId: null,
    notes: '',
    isImportant: false
  };

  patients: any[] = [];
  loading: boolean = false;

  selectedDate: Date | null = null; 
  selectedTimes: string[] = []; // Çoklu saat seçimi
  timeSlots: { time: string, isBooked: boolean }[] = []; 
  existingAppointments: any[] = []; 
  today: Date = new Date();

  constructor(
    private appointmentService: AppointmentService,
    private patientService: PatientService,
    private messageService: MessageService,
    private router: Router,
    private authService: AuthService,
    private primengConfig: PrimeNGConfig
  ) {}

 ngOnInit() {
    this.primengConfig.setTranslation({
      firstDayOfWeek: 1, 
      dayNames: ["Pazar", "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi"],
      dayNamesShort: ["Paz", "Pzt", "Sal", "Çar", "Per", "Cum", "Cmt"],
      dayNamesMin: ["Pz", "Pt", "Sa", "Ça", "Pe", "Cu", "Ct"],
      monthNames: ["Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"],
      monthNamesShort: ["Oca", "Şub", "Mar", "Nis", "May", "Haz", "Tem", "Ağu", "Eyl", "Eki", "Kas", "Ara"],
      today: 'Bugün',
      clear: 'Temizle',
      emptyMessage: 'Kayıt bulunamadı' 
    });

    this.loadPatients();
    this.loadAppointments();
  }

  loadPatients() {
    this.patientService.getList().subscribe({
      next: (res: any) => { this.patients = res.patients || res.items || res.data || []; },
      error: () => { this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Hastalar yüklenemedi.' }); }
    });
  }

  loadAppointments() {
    this.appointmentService.getList().subscribe({
      next: (res: any) => { this.existingAppointments = res.appointments || res.items || res.data || []; },
      error: () => {}
    });
  }

  onDateSelect(event: any) {
    this.selectedDate = event;
    this.selectedTimes = []; 
    this.generateTimeSlots();
  }

  // 🌟 YENİ: Süre (Duration) mantığına göre slotun dolu olup olmadığını kontrol eden metot
  isSlotBooked(h: number, m: number, isToday: boolean, now: Date): boolean {
      if (!this.selectedDate) return false;
      
      // Geçmiş zaman kontrolü
      if (isToday && (h < now.getHours() || (h === now.getHours() && m <= now.getMinutes()))) {
          return true;
      }

      // Döngüdeki bu saati bir Date objesi yap
      const currentSlotTime = new Date(this.selectedDate);
      currentSlotTime.setHours(h, m, 0, 0);

      // Mevcut randevuların Başlangıç ve Bitiş(Süre eklendiğinde) zamanları arasına düşüyor mu?
      for (let app of this.existingAppointments) {
          if (!app.date) continue;
          
          const appStart = new Date(app.date);
          const durationMinutes = app.duration || 30; // Backend'den gelen süre (YOKSA 30)
          const appEnd = new Date(appStart.getTime() + durationMinutes * 60000);

          // Eğer kontrol ettiğimiz saat, randevu aralığındaysa DOLUDUR
          if (currentSlotTime >= appStart && currentSlotTime < appEnd) {
              return true;
          }
      }
      return false;
  }

  generateTimeSlots() {
    this.timeSlots = [];
    const now = new Date(); 
    const isToday = this.selectedDate && this.selectedDate.toDateString() === now.toDateString();

    for (let h = 8; h < 17; h++) {
      for (let m = 0; m < 60; m += 30) {
        if (h === 8 && m === 0) continue; 

        const timeString = `${h.toString().padStart(2, '0')}:${m.toString().padStart(2, '0')}`;
        
        this.timeSlots.push({
          time: timeString,
          isBooked: this.isSlotBooked(h, m, !!isToday, now) // 🌟 Metoda gönderiyoruz
        });
      }
    }
  }

  selectTime(slot: any) {
    if (slot.isBooked) return; 

    const index = this.selectedTimes.indexOf(slot.time);
    if (index > -1) {
        this.selectedTimes.splice(index, 1);
    } else {
        this.selectedTimes.push(slot.time);
    }
    this.selectedTimes.sort(); 
  }

  save() {
    if (!this.appointmentModel.patientId || !this.selectedDate || this.selectedTimes.length === 0) {
      this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Lütfen seçimleri tamamlayın.' });
      return;
    }

    const currentDoctorId = this.authService.getCurrentDoctorId();
    this.loading = true;

    // 🌟 STRATEJİ: En erken saati bul ve kaç blok seçildiğini hesaplayıp süreyi bul
    const sortedTimes = [...this.selectedTimes].sort();
    const startTime = sortedTimes[0]; 
    const calculatedDuration = this.selectedTimes.length * 30; // Örn: 3 blok seçildiyse 90 dakika

    const finalDateTime = new Date(this.selectedDate);  
    const [hours, minutes] = startTime.split(':');
    finalDateTime.setHours(parseInt(hours), parseInt(minutes), 0, 0);

    const year = finalDateTime.getFullYear();
    const month = (finalDateTime.getMonth() + 1).toString().padStart(2, '0');
    const day = finalDateTime.getDate().toString().padStart(2, '0');
    const hoursStr = finalDateTime.getHours().toString().padStart(2, '0');
    const minutesStr = finalDateTime.getMinutes().toString().padStart(2, '0');
    
    const localDateTimeString = `${year}-${month}-${day}T${hoursStr}:${minutesStr}:00`;

    // 🌟 ARTIK TEK BİR KAYIT GÖNDERİYORUZ (Duration Alanı İle Birlikte)
    const payload: any = {
      patientId: this.appointmentModel.patientId,
      date: localDateTimeString,
      duration: calculatedDuration, // Backend'deki yeni alana süreyi yolladık
      notes: this.appointmentModel.notes,
      doctorId: currentDoctorId,
      isImportant: this.appointmentModel.isImportant
    };

    this.appointmentService.add(payload).subscribe({
      next: (res) => {
        const mesaj = calculatedDuration > 30 
          ? `${calculatedDuration} dakikalık randevu başarıyla kaydedildi.` 
          : 'Randevu başarıyla oluşturuldu.';
        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: mesaj });
        setTimeout(() => { this.router.navigate(['/appointments']); }, 1000);
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kaydedilemedi.' });
        this.loading = false;
      }
    });
  }
}