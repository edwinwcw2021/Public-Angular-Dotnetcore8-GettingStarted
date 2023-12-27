import { Directive } from '@angular/core';
import { AbstractControl, Validator, NG_VALIDATORS } from '@angular/forms';

@Directive({
  selector: '[appHkeValidator]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: HkeValidatorDirective,
    multi: true
  }]
})
export class HkeValidatorDirective implements Validator {

  numericRegex = /^\d+(?:\.\d{1,2})?$/;

  validate(control: AbstractControl): { [key: string]: any } | null {
    if (control.value && !this.numericRegex.test(control.value)) {
      return { 'hkeInvalid': true };
    }
    if (<number>control.value >= 715605 && <number>control.value <= 920600) {
      return null;
    } else
      return { 'hkeOutOfBoundInvalid': true };
  }
}
