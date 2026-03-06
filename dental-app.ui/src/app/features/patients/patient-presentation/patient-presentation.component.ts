import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { DropdownModule } from 'primeng/dropdown';
import { ImageModule } from 'primeng/image';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';

import { TreatmentImageService } from '../services/treatment-image.service';

@Component({
  selector: 'app-patient-presentation',
  standalone: true,
  imports: [CommonModule, FormsModule, DropdownModule, ImageModule, CardModule, ButtonModule, DialogModule],
  templateUrl: './patient-presentation.component.html',
  styleUrls: ['./patient-presentation.component.scss']
})
export class PatientPresentationComponent implements OnInit {
  
  patientId: string = '';
  patientFullName: string = 'Hasta Klinik Sunumu'; 
  
  allImages: any[] = [];
  
  angleTypes = [
    { label: 'Ağız İçi (Intraoral)', value: 1 },
    { label: 'Ağız Dışı / Yüz (Extraoral)', value: 2 },
    { label: 'Röntgen (X-Ray)', value: 3 },
    { label: 'Sefalometrik (Cephalometric)', value: 4 }
  ];
  selectedAngle: number = 1;

  dateOptions: any[] = [];
  leftSelectedImage: any = null;
  rightSelectedImage: any = null;

  sliderValue: number = 50; 
  sliderDialogVisible: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private imageService: TreatmentImageService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.patientId = params['id'];
      if (this.patientId) {
        this.fetchImages();
      }
    });
  }

  fetchImages() {
    this.imageService.getPatientImages(this.patientId).subscribe({
      next: (res: any) => {
        // 🌟 PERFORMANS GÜNCELLEMESİ: Doğrudan API sözleşmesine uygun noktadan okuyoruz
        this.allImages = res.images || [];
        this.filterByAngle(); 
      },
      error: (err) => {
        console.error('Fotoğraflar çekilirken bir hata oluştu:', err);
      }
    });
  }

  filterByAngle() {
    if (!this.allImages || this.allImages.length === 0) {
        this.dateOptions = [];
        this.leftSelectedImage = null;
        this.rightSelectedImage = null;
        return;
    }

    const filtered = this.allImages.filter(img => img.type === this.selectedAngle);
    
    filtered.sort((a, b) => new Date(b.recordDate).getTime() - new Date(a.recordDate).getTime());

    this.dateOptions = filtered.map(img => ({
      label: new Date(img.recordDate).toLocaleDateString('tr-TR'),
      value: img
    }));

    if (this.dateOptions.length >= 2) {
        this.leftSelectedImage = this.dateOptions[this.dateOptions.length - 1].value; 
        this.rightSelectedImage = this.dateOptions[0].value; 
    } else if (this.dateOptions.length === 1) {
        this.leftSelectedImage = this.dateOptions[0].value;
        this.rightSelectedImage = null;
    } else {
        this.leftSelectedImage = null;
        this.rightSelectedImage = null;
    }
  }

  getFullImageUrl(imageUrl: string): string {
    if (!imageUrl) return '';
    return imageUrl.replace(/(:\/\/[^\/]+)\/\//g, '$1/');
  }

  openSliderDialog() {
    if (this.leftSelectedImage && this.rightSelectedImage) {
      this.sliderValue = 50;
      this.sliderDialogVisible = true;
    }
  }
}