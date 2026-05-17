import { Component, ElementRef, ViewChild } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { LayoutService } from "./service/app.layout.service";
import { AuthService } from '../core/auth/auth.service';

@Component({
    selector: 'app-topbar',
    templateUrl: './app.topbar.component.html'
})
export class AppTopBarComponent {

    items!: MenuItem[];

    @ViewChild('menubutton') menuButton!: ElementRef;

    @ViewChild('topbarmenubutton') topbarMenuButton!: ElementRef;

    @ViewChild('topbarmenu') menu!: ElementRef;

    // 🌟 AuthService'i buraya inject ettik
    constructor(public layoutService: LayoutService, private authService: AuthService) { }

    // 🌟 HTML'den çağıracağımız çıkış fonksiyonu
    onLogout() {
        this.authService.logout();
    }
}