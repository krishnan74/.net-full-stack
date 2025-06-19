import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function usernameValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    if (value.toUpperCase() === 'ADMIN' || value.toUpperCase() === 'ROOT')
      return {
        usernameInvalid: true,
        message: 'Username cannot be ADMIN or ROOT',
      };
    return null;
  };
}