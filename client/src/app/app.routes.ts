import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { LoginComponent } from './components/login/login';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: '',
    canActivate: [authGuard],
    children: [
      {
        path: '',
        loadComponent: () => import('./components/home/home').then(m => m.HomeComponent)
      },
      {
        path: 'reservable-objects',
        loadComponent: () => import('./components/reservable-objects/reservable-objects').then(m => m.ReservableObjectsComponent)
      },
      {
        path: 'my-reservations',
        loadComponent: () => import('./components/my-reservations/my-reservations').then(m => m.MyReservationsComponent)
      },
      {
        path: 'floor-plan',
        loadComponent: () => import('./components/floor-plan/floor-plan').then(m => m.FloorPlanComponent)
      }
    ]
  },
  {
    path: '**',
    redirectTo: ''
  }
];
