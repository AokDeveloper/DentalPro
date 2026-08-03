import { Component, Input, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MessageService } from 'primeng/api';

import { FileUpload, FileUploadModule } from 'primeng/fileupload';
import { DropdownModule } from 'primeng/dropdown';
import { CalendarModule } from 'primeng/calendar';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';

import { TreatmentImageService } from '../services/treatment-image.service';

@Component({
  selector: 'app-image-upload',
  standalone: true,
  imports: [CommonModule, FormsModule, FileUploadModule, DropdownModule, CalendarModule, InputTextareaModule, ButtonModule, ToastModule],
  providers: [MessageService],
  templateUrl: './image-upload.component.html'
})
export class ImageUploadComponent {
  
  @Input() patientId!: string; 
  @ViewChild('fileUploadControl') fileUploadControl!: FileUpload; // Yüklemeden sonra ekranı temizlemek için
  
  selectedAngle: number = 1;
  recordDate: Date = new Date(); 
  notes: string = '';
  selectedFile: File | null = null;

  angleTypes = [
    { label: 'Ağız İçi (Intraoral)', value: 1 },
    { label: 'Ağız Dışı / Yüz (Extraoral)', value: 2 },
    { label: 'Röntgen (X-Ray)', value: 3 },
    { label: 'Sefalometrik (Cephalometric)', value: 4 },
    { label: 'Profil Fotoğrafı', value: 5 }
  ];

  constructor(
    private imageService: TreatmentImageService,
    private messageService: MessageService
  ) {}

  onFileSelect(event: any) {
    this.selectedFile = event.files[0];
  }

  upload() {
    if (!this.selectedFile || !this.patientId) {
      this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Lütfen bir resim seçin.' });
      return;
    }

    const formData = new FormData();
    
    // 🌟 DİKKAT: İsimler C# Request modelinizle BİREBİR aynı
    formData.append('PatientId', this.patientId); 
    formData.append('File', this.selectedFile); 
    formData.append('Type', this.selectedAngle.toString()); // req.Type'a karşılık gelir
    
    // Tarihi C# DateOnly tipine uyumlu yyyy-mm-dd formatına çeviriyoruz
    const dateString = this.recordDate.toLocaleDateString('en-CA'); 
    formData.append('RecordDate', dateString);
    
    if (this.notes) {
      formData.append('Notes', this.notes);
    }

    this.imageService.uploadPatientImage(formData).subscribe({
      next: (res) => {
        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Fotoğraf başarıyla eklendi!' });
        this.resetForm();
      },
      error: (err) => {
        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Yükleme başarısız oldu.' });
        console.error("Yükleme Hatası:", err);
      }
    });
  }

  resetForm() {
    this.selectedFile = null;
    this.notes = '';
    this.recordDate = new Date();
    // PrimeNG file upload aracının içindeki seçili dosyayı temizler
    if(this.fileUploadControl) {
        this.fileUploadControl.clear();
    }
  }
}