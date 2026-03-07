import { Routes } from '@angular/router';
import { authGuard, guestGuard, roleGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  },
  {
    path: 'login',
    loadComponent: () => import('./pages/login/login').then((m) => m.Login),
    canActivate: [guestGuard],
  },
  {
    path: 'home',
    loadComponent: () => import('./pages/home/home').then((m) => m.Home),
    canActivate: [authGuard],
    data: { title: 'Dashboard' },
  },
  {
    path: 'orders',
    loadComponent: () => import('./pages/home/home').then((m) => m.Home),
    canActivate: [authGuard],
    data: { title: 'Pedidos' },
  },
  {
    path: 'users',
    loadComponent: () => import('./pages/home/home').then((m) => m.Home),
    canActivate: [authGuard, roleGuard],
    data: { title: 'Usuarios', roles: ['admin'] },
  },
  {
    path: 'payments',
    loadComponent: () => import('./pages/home/home').then((m) => m.Home),
    canActivate: [authGuard, roleGuard],
    data: { title: 'Pagos', roles: ['admin', 'manager'] },
  },
  {
    path: '**',
    redirectTo: 'login',
  },
];
