import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Chatmsg } from '../interface/chatmsg.interface';
import { environment } from '@environments/environment';
import { UserInfo } from '../Model/UserInfo';

@Injectable({
  providedIn: 'root'
})
export class ChatmsgService {
  public loginToken: string;
  user: UserInfo;
  constructor(private http: HttpClient) {
    this.user = JSON.parse(window.localStorage.getItem('userinfo'))
    this.user==null?this.loginToken="":this.loginToken=this.user.jwtToken;
  }
  getChatMsgs<T>(roomid: string, status: number, pi: number, ps: number) {
    // const header = new HttpHeaders({
    //   "Authorization": `Bearer ${this.loginToken}`
    // });  
    const httpOptions = {
      headers: new HttpHeaders({
        "Authorization": `Bearer ${this.loginToken}`
      })
    };
    return this.http.get<T>(environment.baseUrls.server + environment.baseUrls.apiUrl + 'ChatMsg/msgs?roomid=' + roomid + '&status=' + status + '&pi=' + pi + '&ps=' + ps, httpOptions);
  }

  getUserId(): string {
    if (this.user != null) {
      return this.user.userId;
    }
  }
}
