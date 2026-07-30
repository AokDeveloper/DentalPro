import { Supervisor } from './../../../core/models/supervisor';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

// PrimeNG Modülleri
import { InputTextModule } from 'primeng/inputtext';
import { InputMaskModule } from 'primeng/inputmask';
import { KeyFilterModule } from 'primeng/keyfilter';
import { CalendarModule } from 'primeng/calendar';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { DropdownModule } from 'primeng/dropdown';
import { MultiSelectModule } from 'primeng/multiselect'; // YENİ EKLENDİ
import { MessageService } from 'primeng/api';

import { PatientService } from '../services/patient.service';
import { SupervisorService } from '../../supervisors/services/supervisor.service';
import { PatientCreate } from '../../../core/models/patients/patientCreate';

@Component({
  selector: 'app-patient-create',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule, 
    RouterModule, 
    InputTextModule, 
    InputMaskModule, 
    KeyFilterModule, 
    CalendarModule, 
    ButtonModule, 
    ToastModule,
    DropdownModule,
    MultiSelectModule // YENİ EKLENDİ
  ],
  providers: [MessageService],
  templateUrl: './patient-create.component.html'
})
export class PatientCreateComponent implements OnInit {

  patientModel: any = {
    firstName: '',
    lastName: '',
    tckn: '',
    phoneNumber: '',
    birthDate: null,
    supervisorId: null,
    selectedCategoryIds: [] // YENİ EKLENDİ
  };

  loading: boolean = false;
  today: Date = new Date();
  supervisors: any[] = [];
  
  groupedCategories: any[] = []; // YENİ EKLENDİ
  categoriesLoading: boolean = true; // YENİ EKLENDİ

  constructor(
    private patientService: PatientService,
    private supervisorService: SupervisorService,
    private messageService: MessageService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadSupervisors();
    this.loadCategories(); // YENİ EKLENDİ
  }

  // YENİ EKLENDİ
loadCategories() {
    this.categoriesLoading = true;
    this.patientService.getGroupedCategories().subscribe({
      next: (response: any) => {
        
        let extractedData = response.categories || response.items || response.data || response.$values || response;

        if (Array.isArray(extractedData)) {
            this.groupedCategories = extractedData;
        } else {
            this.groupedCategories = []; 
        }
        
        this.categoriesLoading = false;
      },
      error: (err) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Kategori listesi yüklenirken bir sorun oluştu.'
        });
        console.error('Categories load error:', err); 
        this.categoriesLoading = false;
      }
    });
  }

  loadSupervisors() {
    this.supervisorService.getList().subscribe({
      next: (response: any) => { 
        this.supervisors = response.supervisors; 
      },
      error: (err) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Danışman hoca listesi yüklenirken bir sorun oluştu.'
        });
        console.error('Supervisor load error:', err);
      }
    });
  }

  save() {
    if (!this.patientModel.firstName || !this.patientModel.lastName || !this.patientModel.phoneNumber) {
      this.messageService.add({ 
        severity: 'warn', 
        summary: 'Uyarı', 
        detail: 'Lütfen Ad, Soyad ve Telefon Numarası alanlarını doldurunuz.' 
      });
      return;
    }

    if (this.patientModel.tckn && this.patientModel.tckn.length !== 11) {
      this.messageService.add({ 
        severity: 'warn', 
        summary: 'Uyarı', 
        detail: 'T.C. Kimlik Numarası 11 haneli olmalıdır.' 
      });
      return;
    }

    this.loading = true;

    // Tarih Dönüşümü (YYYY-MM-DD)
    let formattedDate = '';
    if (this.patientModel.birthDate) {
      const dateObj = new Date(this.patientModel.birthDate);
      const year = dateObj.getFullYear();
      const month = (dateObj.getMonth() + 1).toString().padStart(2, '0');
      const day = dateObj.getDate().toString().padStart(2, '0');
      formattedDate = `${year}-${month}-${day}`; 
    }

    const payload: PatientCreate = {
      firstName: this.patientModel.firstName,
      lastName: this.patientModel.lastName,
      tckn: this.patientModel.tckn,
      phoneNumber: this.patientModel.phoneNumber,
      birthDate: formattedDate,
      supervisorId: this.patientModel.supervisorId,
selectedCategoryIds: this.patientModel.selectedCategoryIds // YENİ EKLENDİ
    } as any;

    this.patientService.createPatient(payload).subscribe({
      next: (res) => {
        this.messageService.add({ 
          severity: 'success', 
          summary: 'Başarılı', 
          detail: 'Hasta kaydı başarıyla oluşturuldu.' 
        });
        
        setTimeout(() => { 
          this.router.navigate(['/patients']); 
        }, 1000);
      },
      error: (err) => {
        this.messageService.add({ 
          severity: 'error', 
          summary: 'Hata', 
          detail: 'Hasta kaydedilirken bir sorun oluştu.' 
        });
        this.loading = false;
      }
    });
  }
}