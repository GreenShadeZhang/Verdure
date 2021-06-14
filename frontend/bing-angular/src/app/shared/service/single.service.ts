import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { UserInfo } from '../Model/UserInfo';

@Injectable({
  providedIn: 'root'
})
export class SingleService {
  public newCpuValue$ = new Subject<string>();
  constructor() {
  }
  public getLogin() {
   return window.localStorage.getItem('userinfo') ? true : false;
  }
  user: UserInfo;
  public getUid() {
    this.user = JSON.parse(window.localStorage.getItem('userinfo'));
    console.log(this.newCpuValue$)
    console.log(this.user.userId)
    this.newCpuValue$.next(this.user.userId);
  }
}
