import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../core/auth/auth.service';

export interface NavItem {
  label: string;
  icon: string;
  route: string;
  roles?: string[];
}

const ALL_NAV_ITEMS: NavItem[] = [
  { label: 'Dashboard', icon: '📊', route: '/home', roles: ['admin', 'manager'] },
  { label: 'Pedidos',   icon: '📦', route: '/orders', roles: ['admin', 'manager'] },
  { label: 'Usuarios',  icon: '👥', route: '/users',    roles: ['admin'] },
  { label: 'Pagos',     icon: '💳', route: '/payments', roles: ['admin'] },
];

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.scss',
})
export class SidebarComponent {
  @Input() collapsed = false;
  @Output() toggleCollapse = new EventEmitter<void>();

  constructor(private authService: AuthService) {}

  get currentUser() {
    return this.authService.getCurrentUser();
  }

  get visibleNavItems(): NavItem[] {
    const role = this.currentUser?.role ?? '';
    return ALL_NAV_ITEMS.filter(
      (item) => !item.roles || item.roles.includes(role),
    );
  }
}
