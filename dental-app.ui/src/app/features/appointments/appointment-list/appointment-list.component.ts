import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { CardModule } from 'primeng/card';
import { TagModule } from 'primeng/tag';
import { TabViewModule } from 'primeng/tabview';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { TooltipModule } from 'primeng/tooltip'; // 🌟 Tooltip eklendi
import { Observable } from 'rxjs';
import { AppointmentService } from '../services/appointment.service';
import { Appointment } from '../../../core/models/appointment';

@Component({
  selector: 'app-appointment-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, TableModule, ButtonModule, InputTextModule, CardModule, TagModule, TabViewModule, DialogModule, DropdownModule, TooltipModule],
  templateUrl: './appointment-list.component.html'
})
export class AppointmentListComponent implements OnInit {
  
  appointments: Appointment[] = [];
  todayAppointments: Appointment[] = [];
  weeklyAppointments: Appointment[] = [];
  loading: boolean = true;

  // 🌟 YENİ: Haftalık Kanban (Board) için değişken
 isDetailedWeeklyView: boolean = false; // Varsayılan olarak sade görünüm başlar
  compactWeeklyAgenda: { dayName: string, date: Date, appointments: Appointment[] }[] = [];
  detailedWeeklyAgenda: { dayName: string, date: Date, slots: { hourLabel: string, appointments: Appointment[] }[] }[] = [];

  isTodayTimelineView: boolean = false; // İlk açılışta tablo gelsin
  todayTimeline: { timeLabel: string, appointments: Appointment[], isPast: boolean }[] = [];
  // DURUM GÜNCELLEME DEĞİŞKENLERİ
  statusDialogVisible: boolean = false;
  selectedAppointment: Appointment | null = null;
  newStatus: number = 1; 
  appointmentNote: string = '';
  detailsDialogVisible: boolean = false;
  viewingAppointment: Appointment | null = null;

  // C# Enum değerlerinizle (1, 2, 3, 4) BİREBİR aynı liste
  statusOptions = [
    { label: 'Planlandı', value: 1 },
    { label: 'Tamamlandı', value: 2 },
    { label: 'İptal Edildi', value: 3 },
    { label: 'Gelmedi', value: 4 }
  ];

  constructor(private appointmentService: AppointmentService) {}

  ngOnInit() {
    this.getAppointments();
  }

  getAppointments() {
    this.loading = true;
    this.appointmentService.getList().subscribe({
      next: (response: any) => {
        this.appointments = response.data || response.appointments || []; 
        this.processAppointments();
        this.loading = false;
      },
      error: (err) => {
        console.error('Randevular çekilemedi:', err);
        this.loading = false;
      }
    });
  }

 processAppointments() {
    const now = new Date();
    
    this.todayAppointments = this.appointments.filter(app => {
        return this.isSameDay(new Date(app.date), now);
    });

    const startOfWeek = this.getStartOfWeek(new Date(now));
    const endOfWeek = this.getEndOfWeek(new Date(now));
    
    this.weeklyAppointments = this.appointments.filter(app => {
      const appDate = new Date(app.date);
      return appDate >= startOfWeek && appDate <= endOfWeek;
    });

    this.generateTodayTimeline(); 
    this.generateWeeklyAgenda(startOfWeek);
  }

  // 🌟 YENİ YARDIMCI METOT: Bir slotun süre (duration) dahilinde dolu olup olmadığını anlar
  private isSlotOccupied(slotTimeLabel: string, appointment: Appointment, currentDate: Date): boolean {
    const appStart = new Date(appointment.date);
    const durationMinutes = (appointment as any).duration || 30; // Süre yoksa 30dk kabul et
    const appEnd = new Date(appStart.getTime() + durationMinutes * 60000); // Başlangıç + Süre = Bitiş

    const [h, m] = slotTimeLabel.split(':').map(Number);
    const slotTime = new Date(currentDate);
    slotTime.setHours(h, m, 0, 0);

    // Eğer kontrol ettiğimiz saat, randevu saatleri aralığındaysa bu slot DOLUDUR
    return slotTime >= appStart && slotTime < appEnd;
  }

  // 🌟 GÜNCEL: Hafta sonlarını (Cumartesi ve Pazar) gizleyen metot
 // 🌟 GÜNCEL: Saatlik matrisi dolduran metot
generateWeeklyAgenda(startOfWeek: Date) {
    this.compactWeeklyAgenda = [];
    this.detailedWeeklyAgenda = [];
    const trDays = ['Pazar', 'Pazartesi', 'Salı', 'Çarşamba', 'Perşembe', 'Cuma', 'Cumartesi'];
    
    const timeSlotsTemplate: string[] = [];
    for (let h = 8; h < 17; h++) {
      for (let m = 0; m < 60; m += 30) {
        if (h === 8 && m === 0) continue; 
        timeSlotsTemplate.push(`${h.toString().padStart(2, '0')}:${m.toString().padStart(2, '0')}`);
      }
    }

    for (let i = 0; i < 7; i++) {
        const currentDate = new Date(startOfWeek);
        currentDate.setDate(startOfWeek.getDate() + i);

        if (currentDate.getDay() === 0 || currentDate.getDay() === 6) continue; 

        const dailyApps = this.weeklyAppointments
            .filter(app => this.isSameDay(new Date(app.date), currentDate))
            .sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());

        this.compactWeeklyAgenda.push({
            dayName: trDays[currentDate.getDay()],
            date: currentDate,
            appointments: dailyApps
        });

        const hourlySlots: { hourLabel: string, appointments: Appointment[] }[] = [];
        
        timeSlotsTemplate.forEach(timeLabel => {
            // 🌟 ARTIK EŞİTLİĞE DEĞİL, ZAMAN ARALIĞINA BAKIYORUZ
            const appsInThisSlot = dailyApps.filter(app => this.isSlotOccupied(timeLabel, app, currentDate));
            hourlySlots.push({ hourLabel: timeLabel, appointments: appsInThisSlot });
        });

        this.detailedWeeklyAgenda.push({
            dayName: trDays[currentDate.getDay()],
            date: currentDate,
            slots: hourlySlots
        });
    }
  }

generateTodayTimeline() {
    this.todayTimeline = [];
    const now = new Date();
    const todayStr = now.toDateString();
    
    const timeSlotsTemplate: string[] = [];
    for (let h = 8; h < 17; h++) {
        for (let m = 0; m < 60; m += 30) {
            if (h === 8 && m === 0) continue; 
            timeSlotsTemplate.push(`${h.toString().padStart(2, '0')}:${m.toString().padStart(2, '0')}`);
        }
    }

    this.todayAppointments.forEach(app => {
        const appDate = new Date(app.date);
        const timeStr = `${appDate.getHours().toString().padStart(2, '0')}:${appDate.getMinutes().toString().padStart(2, '0')}`;
        if (!timeSlotsTemplate.includes(timeStr)) timeSlotsTemplate.push(timeStr);
    });

    timeSlotsTemplate.sort();

    timeSlotsTemplate.forEach(timeLabel => {
        // 🌟 ARTIK EŞİTLİĞE DEĞİL, ZAMAN ARALIĞINA BAKIYORUZ
        const appsInThisSlot = this.todayAppointments.filter(app => this.isSlotOccupied(timeLabel, app, now));

        const [hStr, mStr] = timeLabel.split(':');
        const h = parseInt(hStr, 10);
        const m = parseInt(mStr, 10);
        const isPast = (h < now.getHours()) || (h === now.getHours() && m <= now.getMinutes());

        this.todayTimeline.push({
            timeLabel: timeLabel,
            appointments: appsInThisSlot,
            isPast: isPast
        });
    });
  }
  // 🌟 YENİ: Kanban kartlarının rengini belirleyen metot
  getCardColor(status: number | string | undefined): string {
    const statusNum = Number(status || 1);
    switch (statusNum) {
      case 1: return 'border-blue-500 bg-blue-50 text-blue-900';      
      case 2: return 'border-green-500 bg-green-50 text-green-900';   
      case 3: return 'border-red-500 bg-red-50 text-red-900';         
      case 4: return 'border-orange-500 bg-orange-50 text-orange-900';
      default: return 'border-blue-500 bg-blue-50 text-blue-900';
    }
  }

  openDetailsDialog(appointment: Appointment) {
    this.viewingAppointment = appointment;
    this.detailsDialogVisible = true;
  }

  openStatusDialog(appointment: Appointment) {
    this.selectedAppointment = appointment;
    this.newStatus = appointment.status ? Number(appointment.status) : 1; 
    this.appointmentNote = ''; // Pop-up açılınca eski notu temizle
    this.statusDialogVisible = true;
  }

  // 🌟 MİMARİNİZE UYGUN GÜNCELLEME METODU
  saveStatus() {
    if (!this.selectedAppointment || !this.newStatus) return;

    const appointmentId = this.selectedAppointment.id;
    
    // 🌟 1. Ortak Payload (Artık hepsi 'notes' beklediği için tek sefer tanımlıyoruz)
    const payload = { 
        id: appointmentId, 
        completionNotes: this.appointmentNote 
    };

    // 🌟 2. İsteği tutacak boş bir Observable tanımlıyoruz
    let request$: Observable<any>;

    // 🌟 3. Duruma göre hangi servisin çağrılacağını eşleştiriyoruz
    switch (this.newStatus) {
      case 2: // Tamamlandı
        request$ = this.appointmentService.completeAppointment(appointmentId, payload);
        break;
      case 3: // İptal Edildi
        request$ = this.appointmentService.cancelAppointment(appointmentId, payload);
        break;
      case 4: // Gelmedi
        request$ = this.appointmentService.didntcomeAppointment(appointmentId, payload);
        break;
      default:
        return;
    }

    // 🌟 4. Sadece TEK BİR KERE subscribe oluyoruz!
    request$.subscribe({
      next: () => this.onStatusUpdateSuccess(),
      error: (err) => {
        console.error('Durum güncellenirken hata oluştu:', err);
        alert('Durum güncellenirken hata oluştu.');
      }
    });
  }

  private onStatusUpdateSuccess() {
    // Ekranda durumu güncelle
    this.selectedAppointment!.status = this.newStatus;
    
    // İşlem notunu da ekranda güncelliyoruz ki sayfayı yenilemeden görünsün
    if (this.appointmentNote) {
        this.selectedAppointment!.completionNotes = this.appointmentNote;
    }

    this.processAppointments(); // Listeleri ve haftalık board'u tazele
    this.statusDialogVisible = false;
  }

  getSeverity(status: number | string): "success" | "info" | "warning" | "danger" | "secondary" {
    const statusNum = Number(status);
    switch (statusNum) {
      case 1: return 'info';      // Planlandı -> Mavi
      case 2: return 'success';   // Tamamlandı -> Yeşil
      case 3: return 'danger';    // İptal -> Kırmızı
      case 4: return 'warning';   // Gelmedi -> Turuncu
      default: return 'info';
    }
  }

  getStatusLabel(status: number | string): string {
    const statusNum = Number(status);
    const found = this.statusOptions.find(s => s.value === statusNum);
    return found ? found.label : 'Planlandı';
  }

  isSameDay(d1: Date, d2: Date): boolean {
    return d1.getFullYear() === d2.getFullYear() && d1.getMonth() === d2.getMonth() && d1.getDate() === d2.getDate();
  }

  getStartOfWeek(d: Date) {
    const day = d.getDay(), diff = d.getDate() - day + (day === 0 ? -6 : 1);
    const start = new Date(d.setDate(diff));
    start.setHours(0,0,0,0);
    return start;
  }

  getEndOfWeek(d: Date) {
    const start = this.getStartOfWeek(new Date(d));
    const end = new Date(start.setDate(start.getDate() + 6));
    end.setHours(23,59,59,999);
    return end;
  }
}