import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  constructor(private http:HttpClient) {  
   }
   getBingWallpaper<T>(){
   return this.http.get<T>(environment.baseUrls.server + environment.baseUrls.apiUrl + 'applet/Bing')
   }
}
