import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

// PrimeNG Modülleri
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { CardModule } from 'primeng/card';
import { DialogModule } from 'primeng/dialog';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { TagModule } from 'primeng/tag';         // 🌟 EKLENDİ
import { TooltipModule } from 'primeng/tooltip'; // 🌟 EKLENDİ
import { ConfirmationService, MessageService } from 'primeng/api';

import { PatientService } from '../services/patient.service';
import { PatientList } from '../../../core/models/patients/patientList';
import { ImageUploadComponent } from '../image-upload/image-upload.component';

@Component({
  selector: 'app-patient-list',
  standalone: true,
  imports: [
    CommonModule, 
    RouterModule, 
    TableModule, 
    ButtonModule, 
    InputTextModule, 
    CardModule, 
    DialogModule, 
    ImageUploadComponent,
    ConfirmDialogModule,
    ToastModule,
    TagModule,     // 🌟 EKLENDİ
    TooltipModule  // 🌟 EKLENDİ
  ],
  providers: [ConfirmationService, MessageService],
  templateUrl: './patient-list.component.html'
})
export class PatientListComponent implements OnInit {
  
  patients: PatientList[] = [];
  loading: boolean = true;
  
  uploadDialogVisible: boolean = false;
  selectedPatientIdForUpload: string = '';

  constructor(
    private patientService: PatientService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService
  ) {}

  ngOnInit() {
    this.getPatients();
  }

  getPatients() {
    this.patientService.getList().subscribe({
      next: (response: any) => {
        // 🌟 Güvenli veri çıkarma yöntemi eklendi (response.patients, items, data vb.)
        let extractedData = response.patients || response.items || response.data || response.$values || response;

        if (Array.isArray(extractedData)) {
            this.patients = extractedData;
        } else {
            this.patients = []; 
        }
        
        this.loading = false;
      },
      error: (err) => {
        console.error('Hata:', err);
        this.loading = false;
      }
    });
  }

  openUploadDialog(patientId: string) {
    this.selectedPatientIdForUpload = patientId;
    this.uploadDialogVisible = true;
  }

  handleUploadSuccess() {
    this.uploadDialogVisible = false;
  }

  deletePatient(patient: any) {
    this.confirmationService.confirm({
      icon: 'none',
      message: `<span class="font-semibold text-900 text-lg">${patient.fullName}</span> isimli hastaya ait tüm randevular, tedavi geçmişi ve dosyalar sistemden tamamen silinecektir.<br><br>Bu işlemi kesinlikle geri alamazsınız. Devam etmek istiyor musunuz?`,
      accept: () => {
        this.patientService.delete(patient.id).subscribe({
          next: () => {
            this.messageService.add({ 
              severity: 'success', 
              summary: 'İşlem Başarılı', 
              detail: 'Hasta kaydı ve ilgili veriler kalıcı olarak silindi.' 
            });
            this.getPatients(); 
          },
          error: (err) => {
            this.messageService.add({ 
              severity: 'error', 
              summary: 'Silme Hatası', 
              detail: 'Hasta kaydı silinirken beklenmeyen bir sorun oluştu.' 
            });
            console.error('Delete error:', err);
          }
        });
      }
    });
  }

  // 🌟 EKLENDİ: Tabloda 2'den fazla etiket olduğunda, mouse üzerine gelince tamamını virgülle gösteren metot
  getCategoryNames(categories: any[]): string {
    if (!categories || categories.length === 0) return '';
    return categories.map(c => c.name || c.categoryName || c.label || c).join(', ');
  }
}