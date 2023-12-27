import { Directive } from '@angular/core';
import { AbstractControl, Validator, NG_VALIDATORS } from '@angular/forms';

@Directive({
  selector: '[appHknValidator]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: HknValidatorDirective,
    multi: true
  }]
})
export class HknValidatorDirective {

  numericRegex = /^\d+(?:\.\d{1,2})?$/;

  validate(control: AbstractControl): { [key: string]: any } | null {
    if (control.value && !this.numericRegex.test(control.value)) {
      return { 'hknInvalid': true };
    }
    if (<number>control.value >= 786000 && <number>control.value <= 895400) {
      return null;
    } else
      return { 'hknOutOfBoundInvalid': true };
  }
}
