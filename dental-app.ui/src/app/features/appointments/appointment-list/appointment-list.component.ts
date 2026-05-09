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
import { Observable } from 'rxjs';
import { AppointmentService } from '../services/appointment.service';
import { Appointment } from '../../../core/models/appointment';

@Component({
  selector: 'app-appointment-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, TableModule, ButtonModule, InputTextModule, CardModule, TagModule, TabViewModule, DialogModule, DropdownModule],
  templateUrl: './appointment-list.component.html'
})
export class AppointmentListComponent implements OnInit {
  
  appointments: Appointment[] = [];
  todayAppointments: Appointment[] = [];
  weeklyAppointments: Appointment[] = [];
  loading: boolean = true;

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