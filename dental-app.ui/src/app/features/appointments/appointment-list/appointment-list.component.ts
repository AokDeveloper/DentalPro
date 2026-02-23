import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { CardModule } from 'primeng/card';
import { TagModule } from 'primeng/tag'; // Durum göstergesi için (Renkli etiket)
import { AppointmentService } from '../services/appointment.service';
import { Appointment } from '../../../core/models/appointment';

@Component({
  selector: 'app-appointment-list',
  standalone: true,
  imports: [CommonModule, RouterModule, TableModule, ButtonModule, InputTextModule, CardModule, TagModule],
  templateUrl: './appointment-list.component.html'
})
export class AppointmentListComponent implements OnInit {
  
  appointments: Appointment[] = [];
  loading: boolean = true;

  constructor(private appointmentService: AppointmentService) {}

  ngOnInit() {
    this.getAppointments();
  }

  getAppointments() {
    this.appointmentService.getList().subscribe({
      next: (response: any) => {
        // Backend yapısına göre burayı kontrol edeceğiz (data mı? appointments mı?)
        // Şimdilik standart "data" varsayalım, hasta listesindeki gibi farklıysa düzeltiriz.
        this.appointments = response.data || response.appointments || []; 
        this.loading = false;
      },
      error: (err) => {
        console.error('Randevular çekilemedi:', err);
        this.loading = false;
      }
    });
  }

  // Duruma göre renk belirleyen yardımcı metod
  getSeverity(status: string): "success" | "info" | "warning" | "danger" | undefined {
    switch (status) {
      case 'Completed': return 'success';
      case 'Pending': return 'warning';
      case 'Cancelled': return 'danger';
      default: return 'info';
    }
  }
}