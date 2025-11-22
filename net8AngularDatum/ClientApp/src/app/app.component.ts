import { Component } from '@angular/core';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    standalone: false
})
export class AppComponent {
  title = 'app';

  constructor() {
    const root = document.querySelector('app-root');
    root?.removeAttribute('ng-version');    
  }    
}
