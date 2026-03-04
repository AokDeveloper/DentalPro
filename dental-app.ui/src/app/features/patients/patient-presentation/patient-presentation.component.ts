import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

// PrimeNG Modülleri
import { DropdownModule } from 'primeng/dropdown';
import { ImageModule } from 'primeng/image';
import { CardModule } from 'primeng/card';

// Kendi servis yolunuzu projenize göre güncelleyin
import { TreatmentImageService } from '../services/treatment-image.service';


@Component({
  selector: 'app-patient-presentation',
  standalone: true,
  imports: [CommonModule, FormsModule, DropdownModule, ImageModule, CardModule],
  templateUrl: './patient-presentation.component.html'
})
export class PatientPresentationComponent implements OnInit {

  patientId: string = '';
  patientFullName: string = 'Hasta Sunumu'; 
  
  allImages: any[] = [];       // Backend'den gelen tüm fotoğraflar
  filteredImages: any[] = [];  // Sadece seçilen açıya ait fotoğraflar
  
  // Sol ve Sağ ekran için seçilen fotoğraf objeleri
  leftSelectedImage: any = null;
  rightSelectedImage: any = null;

  // C# tarafındaki Enum değerlerinize göre burayı düzenleyebilirsiniz
  angleTypes = [
    { label: 'Ağız İçi (Intraoral)', value: 1 },
    { label: 'Ağız Dışı / Yüz (Extraoral)', value: 2 },
    { label: 'Röntgen (X-Ray)', value: 3 },
    { label: 'Sefalometrik (Cephalometric)', value: 4 }
  ];
  
  selectedAngle: number = 1; // Varsayılan olarak 'Profil' açılışta gelsin

  // Açılır kutuda (Dropdown) görünecek seçenekler
  dateOptions: any[] = [];

  constructor(
    private route: ActivatedRoute,
    private imageService: TreatmentImageService
  ) {}

  ngOnInit() {
    // URL'den hasta ID'sini alıyoruz (Örn: /patients/presentation/123)
    this.patientId = this.route.snapshot.paramMap.get('id') || '';
    if(this.patientId) {
       this.fetchImages();
    }
  }

  fetchImages() {
    this.imageService.getPatientImages(this.patientId).subscribe({
      next: (res: any) => {
        this.allImages = res.images || [];
        this.filterByAngle(); // Veriler gelince varsayılan açıya göre filtrele
      },
      error: (err) => console.error("Fotoğraflar çekilemedi", err)
    });
  }

  // Doktor üstten açıyı değiştirdiğinde tetiklenir
  filterByAngle() {
    // 1. Sadece seçilen açıdaki fotoğrafları bul
    this.filteredImages = this.allImages.filter(img => img.type === this.selectedAngle);
    
    // 2. Fotoğrafları çekim tarihine (RecordDate) göre ESKİDEN YENİYE sırala
    this.filteredImages.sort((a, b) => new Date(a.recordDate).getTime() - new Date(b.recordDate).getTime());

    // 3. Dropdown için verileri hazırla
    this.dateOptions = this.filteredImages.map(img => {
      // Tarihi TR formatına çevirelim (Örn: 15.05.2024)
      const d = new Date(img.recordDate);
      const formattedDate = d.toLocaleDateString('tr-TR');
      
      return {
        label: `${formattedDate} Seansı`, // Dropdown'da görünecek yazı
        value: img // Seçildiğinde ImageUrl değil, tüm objeyi alıyoruz ki notları da yazdıralım
      };
    });

    // 4. Ekrana ilk açıldığında otomatik olarak İlk fotoğrafı SOLA, Son fotoğrafı SAĞA koy
    if (this.dateOptions.length > 0) {
      this.leftSelectedImage = this.dateOptions[0].value; // Tedavi başı
      this.rightSelectedImage = this.dateOptions[this.dateOptions.length - 1].value; // En güncel hali
    } else {
      this.leftSelectedImage = null;
      this.rightSelectedImage = null;
    }
    
  }
  getFullImageUrl(imageUrl: string): string {
    if (!imageUrl) return '';
    
    // Eğer veritabanından gelen adres zaten tam ise, içindeki hatalı çift slaçları temizle
    // Örnek: "localhost:9000//dental-images" -> "localhost:9000/dental-images"
    // (http:// kısmındaki çift slacı bozmamak için replace kullanıyoruz)
    return imageUrl.replace(/(:\/\/[^\/]+)\/\//g, '$1/');
  }
}