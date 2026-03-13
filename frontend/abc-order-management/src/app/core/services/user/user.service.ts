import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, lastValueFrom, map } from 'rxjs';
import { User, AuthUser, UserRegister } from '../../../shared/types/user/user.interface';
import environment from '../../../../../environments/environment';
@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly API_URL = environment.apiUrlUsers + '/users';

  constructor(private http: HttpClient,) {}

  async getUsers(): Promise<UserRegister[]> {
    return await lastValueFrom(this.http.get<UserRegister[]>(this.API_URL));
  }
}
