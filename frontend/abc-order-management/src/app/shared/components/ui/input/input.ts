import { Component, Input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormErrorService } from '../../../../core/services/form-error/form-error.service';

@Component({
  selector: 'ui-input',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './input.html',
  styleUrl: './input.scss',
})
export class InputComponent {
  @Input() label = '';
  @Input() type: 'text' | 'email' | 'password' | 'number' = 'text';
  @Input() placeholder = '';
  @Input() control!: FormControl;

  showPassword = false;

  constructor(private formErrorService: FormErrorService) {}

  get errorMessage(): string | null {
    if (this.control?.touched) {
      return this.formErrorService.getErrorMessage(this.control.errors);
    }
    return null;
  }

  get inputType(): string {
    if (this.type === 'password') {
      return this.showPassword ? 'text' : 'password';
    }
    return this.type;
  }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }
}
