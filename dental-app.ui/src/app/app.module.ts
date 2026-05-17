import { NgModule } from '@angular/core';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { AppLayoutModule } from './layout/app.layout.module';

// 🌟 YENİ YÖNTEM: Modern HTTP Sağlayıcıları
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './core/interceptors/auth.interceptor'; // Dosya yolunuza göre ayarlayın

@NgModule({
    declarations: [
        AppComponent
    ],
    imports: [
        AppRoutingModule,
        AppLayoutModule
        // 🌟 DİKKAT: HttpClientModule BURADAN TAMAMEN SİLİNDİ!
    ],
    providers: [
        { provide: LocationStrategy, useClass: HashLocationStrategy },
        
        // 🌟 İŞTE SİHİR BURADA: Gümrük memurunu (Interceptor) sisteme mühürlüyoruz
        provideHttpClient(
            withInterceptors([authInterceptor])
        )
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }