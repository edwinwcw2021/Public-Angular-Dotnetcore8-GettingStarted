import { Directive } from '@angular/core';
import { AbstractControl, Validator, NG_VALIDATORS } from '@angular/forms';

@Directive({
  selector: '[appMinuteValidator]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: MinuteValidatorDirective,
    multi: true
  }]

})
export class MinuteValidatorDirective implements Validator {
  numericRegex = /^\d+(?:\.\d{1,2})?$/;

  validate(control: AbstractControl): { [key: string]: any } | null {
    if (control.value && !this.numericRegex.test(control.value)) {
      return { 'minutesInvalid': true };
    }
    if (<number>control.value >= 0 && <number>control.value < 60) {
      return null;
    } else
      return { 'minutesOutOfBoundInvalid': true };
  }

}
