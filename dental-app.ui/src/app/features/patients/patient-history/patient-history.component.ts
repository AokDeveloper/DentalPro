import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { TooltipModule } from 'primeng/tooltip';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

import { PatientService } from '../services/patient.service';

@Component({
  selector: 'app-patient-history',
  standalone: true,
  imports: [CommonModule, RouterModule, TableModule, ButtonModule, CardModule, TooltipModule, ToastModule],
  providers: [MessageService],
  templateUrl: './patient-history.component.html'
})
export class PatientHistoryComponent implements OnInit {
  patientId: string = '';
  historyRecords: any[] = [];
  loading: boolean = true;

  constructor(
    private route: ActivatedRoute,
    private patientService: PatientService,
    private messageService: MessageService
  ) {}

  ngOnInit() {
    // URL'deki :id parametresini yakalıyoruz
    this.patientId = this.route.snapshot.paramMap.get('id') || '';
    if (this.patientId) {
      this.loadHistory();
    }
  }

  loadHistory() {
    this.loading = true;
    this.patientService.getCompletedAppointments(this.patientId).subscribe({
      next: (response: any) => {
        // API'den dönen JSON'daki 'completedAppointments' dizisini alıyoruz
        this.historyRecords = response.completedAppointments || [];
        this.loading = false;
      },
      error: (err) => {
        this.messageService.add({ 
          severity: 'error', 
          summary: 'Hata', 
          detail: 'Geçmiş randevular yüklenirken bir sorun oluştu.' 
        });
        console.error('History load error:', err);
        this.loading = false;
      }
    });
  }
}