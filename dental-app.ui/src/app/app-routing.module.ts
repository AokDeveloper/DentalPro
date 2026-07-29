import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppLayoutComponent } from './layout/app.layout.component';
import { authGuard } from './core/auth/auth.guard';
import { PatientHistoryComponent } from './features/patients/patient-history/patient-history.component';

const routes: Routes = [

    { 
        // 1. TAM SAYFA LOGIN EKRANI (Menü ve Topbar Olmadan)
        path: 'login', 
        loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent) 
    },
    
    {
        path: '', component: AppLayoutComponent,
        canActivate: [authGuard],
        children: [
            { 
                path: '', 
                // Yeni oluşturduğumuz Dashboard'a lazy loading ile gidiyoruz
                loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent) 
            },

            { 
    path: 'patients', 
    loadComponent: () => import('./features/patients/patient-list/patient-list.component').then(m => m.PatientListComponent) 
},
{ 
    path: 'appointments/create', 
    loadComponent: () => import('./features/appointments/appointment-create/appointment-create.component').then(m => m.AppointmentCreateComponent) 
},
{ 
    path: 'appointments', 
    loadComponent: () => import('./features/appointments/appointment-list/appointment-list.component').then(m => m.AppointmentListComponent) 
},
{ 
  path: 'patients/presentation/:id', 
  loadComponent: () => import('./features/patients/patient-presentation/patient-presentation.component').then(m => m.PatientPresentationComponent) 
},
{ 
    path: 'patients/create', 
    loadComponent: () => import('./features/patients/patient-create/patient-create.component').then(m => m.PatientCreateComponent) 
},
{ 
    path: 'patients/history/:id', 
    loadComponent: () => import('./features/patients/patient-history/patient-history.component').then(m => m.PatientHistoryComponent) 
}

            // İleride Hastalar modülünü buraya ekleyeceğiz
            // { path: 'patients', ... }
        ]
    },  
    // Login sayfası layout dışı olacak
    // { path: 'auth', ... },
    { path: '**', redirectTo: '' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'enabled', anchorScrolling: 'enabled', onSameUrlNavigation: 'reload' })],
    exports: [RouterModule]
})
export class AppRoutingModule { }