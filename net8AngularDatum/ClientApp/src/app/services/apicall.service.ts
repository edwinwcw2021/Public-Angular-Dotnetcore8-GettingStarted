import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HKG, Geo } from '../model/model';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class ApicallService {

  constructor(private http: HttpClient) {
  }

  hkgtoGeo(req: HKG): Observable<Geo> {
    var formData: any = new FormData();
    formData.append('x', req.x);
    formData.append('y', req.y);
    const sUrl = `/api/Datum/getGEOByHKG`;
    return this.http.post<Geo>(sUrl, formData);
  }

  geotohkg(req: Geo): Observable<HKG> {
    var formData: any = new FormData();
    formData.append('latitudeDeg', req.latitudeDeg);
    formData.append('latitudeMin', req.latitudeMin);
    formData.append('longitudeDeg', req.longitudeDeg);
    formData.append('longitudeMin', req.longitudeMin);
    const sUrl = `/api/Datum/getHKGByGEO`;
    return this.http.post<HKG>(sUrl, formData);
  }
}
