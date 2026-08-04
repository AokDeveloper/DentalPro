import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card'; // Card modülünü ekleyelim

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, CardModule], // Import'u unutmayalım
  template: `
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <h5>DentalApp Dashboard</h5>
                <p>Sistem başarıyla çalışıyor ve layout oturmuş durumda.</p>
            </div>
        </div>

        <div class="col-12 md:col-6 lg:col-3">
            <div class="card mb-0">
                <div class="flex justify-content-between mb-3">
                    <div>
                        <span class="block text-500 font-medium mb-3">Sisteme Kayıtlı Hasta</span>
                        <div class="text-900 font-medium text-xl">152</div>
                    </div>
                    <div class="flex align-items-center justify-content-center bg-blue-100 border-round" style="width:2.5rem;height:2.5rem">
                        <i class="pi pi-users text-blue-500 text-xl"></i>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-12 md:col-6 lg:col-3">
            <div class="card mb-0">
                <div class="flex justify-content-between mb-3">
                    <div>
                        <span class="block text-500 font-medium mb-3">Bugünkü Randevular</span>
                        <div class="text-900 font-medium text-xl">25</div>
                    </div>
                    <div class="flex align-items-center justify-content-center bg-orange-100 border-round" style="width:2.5rem;height:2.5rem">
                        <i class="pi pi-calendar text-orange-500 text-xl"></i>
                    </div>
                </div>
            </div>
        </div>
    </div>


  `
})
export class DashboardComponent {}