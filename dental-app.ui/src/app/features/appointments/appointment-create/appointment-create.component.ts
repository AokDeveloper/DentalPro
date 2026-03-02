import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

// PrimeNG modülleri
import { DropdownModule } from 'primeng/dropdown';
import { CalendarModule } from 'primeng/calendar';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { MessageService, PrimeNGConfig } from 'primeng/api';

import { AppointmentService } from '../services/appointment.service';
import { PatientService } from '../../patients/services/patient.service';
import { AuthService } from '../../../core/auth/auth.service';

@Component({
  selector: 'app-appointment-create',
  standalone: true,
  imports: [CommonModule, FormsModule, DropdownModule, CalendarModule, InputTextareaModule, ButtonModule, ToastModule],
  providers: [MessageService],
  templateUrl: './appointment-create.component.html'
})
export class AppointmentCreateComponent implements OnInit {

  appointmentModel: any = {
    patientId: null,
    notes: ''
  };

  patients: any[] = [];
  loading: boolean = false;

  // --- ZAMAN ÇİZELGESİ DEĞİŞKENLERİ ---
  selectedDate: Date | null = null; // Sadece seçilen gün
  selectedTime: string | null = null; // Seçilen saat (Örn: "10:30")
  timeSlots: { time: string, isBooked: boolean }[] = []; 
  existingAppointments: any[] = []; // Çakışma kontrolü için tüm randevular
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
    // 🌟 TÜRKÇE TAKVİM AYARLARI 
    this.primengConfig.setTranslation({
      firstDayOfWeek: 1, // Hafta Pazartesi başlar (Pazar 0'dır)
      dayNames: ["Pazar", "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi"],
      dayNamesShort: ["Paz", "Pzt", "Sal", "Çar", "Per", "Cum", "Cmt"],
      dayNamesMin: ["Pz", "Pt", "Sa", "Ça", "Pe", "Cu", "Ct"],
      monthNames: ["Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"],
      monthNamesShort: ["Oca", "Şub", "Mar", "Nis", "May", "Haz", "Tem", "Ağu", "Eyl", "Eki", "Kas", "Ara"],
      today: 'Bugün',
      clear: 'Temizle',
      emptyMessage: 'Kayıt bulunamadı' // Açılır kutular boşsa çıkacak mesaj
    });

    this.loadPatients();
    this.loadAppointments();
  }

  loadPatients() {
    this.patientService.getList().subscribe({
      next: (res: any) => {
        // 'res.patients' diyerek kutunun içindeki gerçek listeyi alıyoruz
        this.patients = res.patients || res.items || res.data || []; 
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Hastalar yüklenirken bir sorun oluştu.' });
      }
    });
  }

  loadAppointments() {
    this.appointmentService.getList().subscribe({
      next: (res: any) => {
        // 'res.appointments' diyerek kutunun içindeki gerçek listeyi alıyoruz
        this.existingAppointments = res.appointments || res.items || res.data || [];
      },
      error: (err) => {
        // İsteğe bağlı olarak kullanıcıya hata mesajı gösterebilirsiniz
      }
    });
  }

  // Takvimden gün seçildiğinde tetiklenir
  onDateSelect(event: any) {
    this.selectedDate = event;
    this.selectedTime = null; 
    this.generateTimeSlots();
  }

  // 08:00 - 17:00 arası 30 dakikalık dilimler oluşturur
  // 08:30 - 17:00 arası 30 dakikalık dilimler oluşturur
 generateTimeSlots() {
    this.timeSlots = [];
    const startHour = 8;  // Mesai başlangıcı (Döngü 8'den başlar)
    const endHour = 17;   // Mesai bitişi

    const now = new Date(); // Şu anki bilgisayar saati
    // Kullanıcının takvimden seçtiği gün, "bugün" mü?
    const isToday = this.selectedDate && this.selectedDate.toDateString() === now.toDateString();

    // Seçtiğimiz gündeki randevuların saatlerini bulalım
    const bookedHours = this.existingAppointments
      .filter(app => {
        if (!app.date) return false;
        const appDate = new Date(app.date);
        return this.selectedDate && appDate.toDateString() === this.selectedDate.toDateString();
      })
      .map(app => {
        const appDate = new Date(app.date);
        const hours = appDate.getHours().toString().padStart(2, '0');
        const mins = appDate.getMinutes().toString().padStart(2, '0');
        return `${hours}:${mins}`; 
      });

    // Döngü ile butonları oluştur
    for (let h = startHour; h < endHour; h++) {
      for (let m = 0; m < 60; m += 30) {
        
        // Kural 1: 08:00 butonunu oluşturma, atla.
        if (h === 8 && m === 0) {
          continue; 
        }

        // 🌟 YENİ KURAL 2: Eğer seçilen gün "bugün" ise, geçmiş saatleri engelle
        let isPast = false;
        if (isToday) {
          // Eğer döngüdeki saat, şu anki saatten küçükse VEYA 
          // saatler eşit ama döngüdeki dakika şu anki dakikadan küçük/eşitse bu saat GEÇMİŞTİR.
          if (h < now.getHours() || (h === now.getHours() && m <= now.getMinutes())) {
            isPast = true;
          }
        }

        const timeString = `${h.toString().padStart(2, '0')}:${m.toString().padStart(2, '0')}`;
        
        this.timeSlots.push({
          time: timeString,
          // Buton ne zaman pasif olacak? -> Ya veritabanında doluysa VEYA saati geçmişse!
          isBooked: bookedHours.includes(timeString) || isPast 
        });
      }
    }
  }

  // Saat butonuna tıklandığında
  selectTime(slot: any) {
    if (slot.isBooked) return; // Doluysa hiçbir şey yapma
    this.selectedTime = slot.time;
  }

  save() {
    if (!this.appointmentModel.patientId || !this.selectedDate || !this.selectedTime) {
      this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Lütfen hasta, tarih ve saat seçiniz.' });
      return;
    }

    const currentDoctorId = this.authService.getCurrentDoctorId();
    if (!currentDoctorId) {
      this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Oturum bilginiz bulunamadı.' });
      return;
    }

    const finalDateTime = new Date(this.selectedDate);  
    const [hours, minutes] = this.selectedTime.split(':');
    finalDateTime.setHours(parseInt(hours), parseInt(minutes), 0, 0);

    // Angular'ın saati UTC'ye (-3 saat) çevirmesini engellemek için
    // Tarihi manuel olarak YYYY-MM-DDTHH:mm:00 formatında bir metne çeviriyoruz.
    const year = finalDateTime.getFullYear();
    const month = (finalDateTime.getMonth() + 1).toString().padStart(2, '0');
    const day = finalDateTime.getDate().toString().padStart(2, '0');
    const hoursStr = finalDateTime.getHours().toString().padStart(2, '0');
    const minutesStr = finalDateTime.getMinutes().toString().padStart(2, '0');
    
    // Z (Zulu/UTC) harfini koymuyoruz ki C# bunu direkt yerel saat olarak kabul etsin
    const localDateTimeString = `${year}-${month}-${day}T${hoursStr}:${minutesStr}:00`;

    const payload: any = {
      patientId: this.appointmentModel.patientId,
      date: localDateTimeString, // Artık ham obje değil, formatlanmış metin gönderiyoruz
      notes: this.appointmentModel.notes,
      doctorId: currentDoctorId 
    };

    this.loading = true;

    this.appointmentService.add(payload).subscribe({
      next: (res) => {
        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Randevu başarıyla oluşturuldu.' });
        setTimeout(() => { this.router.navigate(['/appointments']); }, 1000);
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kaydedilemedi.' });
        this.loading = false;
      }
    });
  }
}