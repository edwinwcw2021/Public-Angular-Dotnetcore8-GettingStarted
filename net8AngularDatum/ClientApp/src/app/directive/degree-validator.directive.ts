import { Directive } from '@angular/core';
import { AbstractControl, Validator, NG_VALIDATORS } from '@angular/forms';
@Directive({
  selector: '[appDegreeValidator]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: DegreeValidatorDirective,
    multi: true
  }]
})
export class DegreeValidatorDirective implements Validator {
  numericRegex = /\d/;

  validate(control: AbstractControl): { [key: string]: any } | null {
    if (control.value && !this.numericRegex.test(control.value)) {
      return { 'degreeInvalid': true };
    }
    if (<number>control.value >= 113 && <number>control.value <= 114) {
      return null;
    } else
      return { 'degreeOutOfBoundInvalid': true };
  }

}
