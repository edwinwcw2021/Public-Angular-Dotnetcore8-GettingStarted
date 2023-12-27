import { Component } from '@angular/core';
import { ApicallService } from '../services/apicall.service';
import { HKG, Geo } from '../model/model';
import { catchError } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { from } from 'rxjs/internal/observable/from';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  HKGData: HKG = { x: undefined, y: undefined };
  GeoData: Geo = { latitudeDeg: 22, latitudeMin: undefined, longitudeDeg: undefined, longitudeMin: undefined };
  convertFrom: string = "geo";

  constructor(private apiServer: ApicallService) {

  }

  ngAfterViewInit(): void {
    this.setDefault();
  }
  ngOnInit(): void {
  }

  setDefault(): void {
    this.HKGData = { x: undefined, y: undefined };
    this.GeoData = { latitudeDeg: 22, latitudeMin: undefined, longitudeDeg: undefined, longitudeMin: undefined };
  }

  selectedOption(opt: string): boolean {
    return (opt == this.convertFrom);
  }

  btnConvert(): void {
    if (this.convertFrom == 'geo') {
      console.log('selected geo')
      this.apiServer.geotohkg(this.GeoData).pipe<HKG>(
        catchError(error => {
          if (!environment.production)
            console.log(error);
          return from([]);
        }))
        .subscribe(data => {
          if (data != null) {
            if (!environment.production)
              console.log(data);
            this.HKGData = <HKG>data;
          } else {
            if (!environment.production)
              console.log('no data error');
          }
        });
    } else {
      console.log('selected hkg')
      this.apiServer.hkgtoGeo(this.HKGData).pipe<Geo>(
        catchError(error => {
          if (!environment.production)
            console.log(error);
          return from([]);
        }))
        .subscribe(data => {
          if (data != null) {
            if (!environment.production)
              console.log(data);
            this.GeoData = <Geo>data;
          } else {
            if (!environment.production)
              console.log('no data error');
          }
        });
    }
  }

}
