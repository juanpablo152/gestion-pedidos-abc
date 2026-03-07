import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, lastValueFrom, map } from 'rxjs';
import { User, AuthUser, UserRegister } from '../../../shared/types/user/user.interface';
import environment from '../../../../../environments/environment';
@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly STORAGE_KEY = 'auth_user';
  private readonly API_URL = environment.apiUrl + '/users';

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

  async getUsers(): Promise<UserRegister[]> {
    return await lastValueFrom(this.http.get<UserRegister[]>(this.API_URL));
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
