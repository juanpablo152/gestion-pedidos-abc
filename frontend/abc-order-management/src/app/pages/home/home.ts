import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TableComponent } from '../../shared/components/ui/table/table';
import { TableColumn } from '../../shared/types/table/table.interface';
import { UserService } from '../../core/services/user-services/user.service';
import { MainLayout } from '../../layouts/main-layout/main-layout';
import { UserRegister } from '../../shared/types/user/user.interface';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [TableComponent, MainLayout],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home implements OnInit {
  loading = signal(true);
  users = signal<UserRegister[]>([]);
  pageTitle = signal('Dashboard');

  columns: TableColumn<any>[] = [
    { key: 'id', label: '#' },
    { key: 'name', label: 'Nombre' },
    { key: 'website', label: 'Sitio web' },
    { key: 'email', label: 'correo electrónico' },
    { key: 'phone', label: 'teléfono' },
  ];

  constructor(
    private userService: UserService,
    private router: Router,
    private route: ActivatedRoute,
  ) {}

  get currentUser() {
    return this.userService.getCurrentUser();
  }

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.pageTitle.set(data['title'] ?? 'Dashboard');
    });
    this.getUsers();
  }

  logout(): void {
    this.userService.logout();
    this.router.navigate(['/login']);
  }

  async getUsers(): Promise<void> {
    try {
      this.loading.set(true);
      this.users.set(await this.userService.getUsers());
      this.loading.set(false);
    } catch (error) {
      this.loading.set(false);
      console.error(error);
    }
  }
}
