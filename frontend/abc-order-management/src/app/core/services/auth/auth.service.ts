import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { AuthUser, User } from '../../../shared/types/user/user.interface';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly STORAGE_KEY = 'auth_user';

  constructor(private http: HttpClient,) {}

  async login(username: string, password: string): Promise<AuthUser | undefined> {
    const users = await lastValueFrom(this.http.get<User[]>('/data/users.json'));
    const found = users.find(
      (u) => u.username === username && u.password === password,
    );
    if (found) {
      const authUser: AuthUser = {
        id: found.id,
        name: found.name,
        username: found.username,
        role: found.role,
      };
      sessionStorage.setItem(this.STORAGE_KEY, JSON.stringify(authUser));
      return authUser;
    }
    return undefined;
  }

  logout(): void {
    sessionStorage.removeItem(this.STORAGE_KEY);
  }

  getCurrentUser(): AuthUser | null {
    const stored = sessionStorage.getItem(this.STORAGE_KEY);
    return stored ? JSON.parse(stored) : null;
  }

  isAuthenticated(): boolean {
    return !!this.getCurrentUser();
  }
}

