import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { MinuteValidatorDirective } from './directive/minute-validator.directive';
import { DegreeValidatorDirective } from './directive/degree-validator.directive';
import { HkeValidatorDirective } from './directive/hke-validator.directive';
import { HknValidatorDirective } from './directive/hkn-validator.directive';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './modules/material.module';
import { AboutComponent } from './about/about.component';

@NgModule({ declarations: [
        MinuteValidatorDirective,
        DegreeValidatorDirective,
        HkeValidatorDirective,
        HknValidatorDirective,
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        AboutComponent
    ],
    bootstrap: [AppComponent], imports: [MaterialModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule.forRoot([
            { path: '', component: HomeComponent, pathMatch: 'full' },
            { path: 'about', component: AboutComponent }
        ]),
        BrowserAnimationsModule], providers: [provideHttpClient(withInterceptorsFromDi())] })
export class AppModule { }
