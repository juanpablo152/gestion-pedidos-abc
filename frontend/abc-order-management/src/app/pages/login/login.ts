import { Component, signal } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../core/services/user/user.service';
import { InputComponent } from '../../shared/components/ui/input/input';
import { ButtonComponent } from '../../shared/components/ui/button/button';
import { AuthLayout } from '../../layouts/auth-layout/auth-layout';
import { AuthService } from '../../core/services/auth/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, InputComponent, ButtonComponent, AuthLayout],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  form = new FormGroup({
    username: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)]),
  });

  loading = signal(false);
  errorMessage = signal('');

  constructor(
    private authService: AuthService,
    private router: Router,
  ) {}

  get usernameControl(): FormControl {
    return this.form.get('username') as FormControl;
  }

  get passwordControl(): FormControl {
    return this.form.get('password') as FormControl;
  }

  async onSubmit(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    try {
      this.loading.set(true);
      this.errorMessage.set('');
      const { username, password } = this.form.value;
      const user = await this.authService.login(username!, password!);
      this.loading.set(false);
      if (!user) {
        this.errorMessage.set('Credenciales incorrectas. Verifica tu usuario y contraseña.');
        return;
      }
      this.router.navigate(['/home']);
    } catch (error) {
      this.loading.set(false);
      this.errorMessage.set('Error al iniciar sesión. Intenta de nuevo.');
    }
  }
}
