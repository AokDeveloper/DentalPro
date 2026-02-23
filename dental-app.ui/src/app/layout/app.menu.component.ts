import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { LayoutService } from './service/app.layout.service';

@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html'
})
export class AppMenuComponent implements OnInit {

    model: any[] = [];

    constructor(public layoutService: LayoutService) { }

    ngOnInit() {
        this.model = [
            {
                label: 'Ana Menü',
                items: [
                    { label: 'Dashboard', icon: 'pi pi-fw pi-home', routerLink: ['/'] }
                ]
            },
            {
                label: 'Hasta Yönetimi',
                items: [
                    { label: 'Hastalar', icon: 'pi pi-fw pi-users', routerLink: ['/patients'] },
                    { label: 'Randevular', icon: 'pi pi-fw pi-calendar', routerLink: ['/appointments'] }
                ]
            }
        ];
    }
}