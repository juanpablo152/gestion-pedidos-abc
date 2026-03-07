import { Component, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { TableComponent } from '../../shared/components/ui/table/table';
import { TableColumn } from '../../shared/types/table/table.interface';
import { UserService } from '../../core/services/user-services/user.service';
import { MainLayout } from '../../layouts/main-layout/main-layout';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [TableComponent, MainLayout],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home implements OnInit {
  loading = signal(true);

  columns: TableColumn<any>[] = [
    { key: 'id', label: '#', width: '60px' },
    { key: 'userId', label: 'Usuario', width: '90px' },
    { key: 'title', label: 'Título' },
    { key: 'body', label: 'Descripción' },
  ];

  constructor(
    private userService: UserService,
    private router: Router,
  ) {}

  get currentUser() {
    return this.userService.getCurrentUser();
  }

  ngOnInit(): void {
    this.loading.set(false);
  }

  logout(): void {
    this.userService.logout();
    this.router.navigate(['/login']);
  }

  async getUsers(): Promise<void> {
    try {
      
    } catch (error) {
      console.error(error);
    }
  }
}
