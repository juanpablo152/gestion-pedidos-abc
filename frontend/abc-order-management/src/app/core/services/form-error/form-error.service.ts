import { Injectable } from '@angular/core';
import { ValidationErrors } from '@angular/forms';

@Injectable({
  providedIn: 'root',
})
export class FormErrorService {
  private errorMessages: Record<string, (error: any) => string> = {
    required: () => 'Este campo es requerido',
    min: (error) => `El valor debe ser mayor o igual a ${error.min}`,
    max: (error) => `El valor debe ser menor o igual a ${error.max}`,
    minlength: (error) => `El texto debe tener al menos ${error.requiredLength} caracteres`,
    maxlength: (error) => `El texto no debe exceder ${error.requiredLength} caracteres`,
    email: () => 'Formato de correo electrónico inválido',
    pattern: () => 'Formato inválido',
  };

  getErrorMessage(errors: ValidationErrors | null | undefined): string | null {
    if (!errors) {
      return null;
    }
    const firstErrorKey = Object.keys(errors)[0];
    const getMessage = this.errorMessages[firstErrorKey];
    if (getMessage) {
      return getMessage(errors[firstErrorKey]);
    }
    return 'Error de validación';
  }
}
