import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { CardModule } from 'primeng/card';
// IconField ve InputIcon importları YOK. Çünkü Sakai sürümünde bunlar yok.

import { PatientService } from '../services/patient.service';
import { Patient } from '../../../core/models/patient';

@Component({
  selector: 'app-patient-list',
  standalone: true,
  imports: [CommonModule, TableModule, ButtonModule, InputTextModule, CardModule], 
  templateUrl: './patient-list.component.html'
})
export class PatientListComponent implements OnInit {
  
  patients: Patient[] = [];
  loading: boolean = true;

  constructor(private patientService: PatientService) {}

  ngOnInit() {
    this.getPatients();
  }

 getPatients() {
    
    this.patientService.getList().subscribe({
      next: (response: any) => {
 
        this.patients = response.patients; 
        this.loading = false;
      },
      error: (err) => {
        console.error('Hata:', err);
        this.loading = false;
      }
    });
  }
}