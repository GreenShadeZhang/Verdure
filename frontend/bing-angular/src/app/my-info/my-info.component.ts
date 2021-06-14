import { Component, OnInit, ErrorHandler, Injectable, Injector, NgZone } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { environment } from '@environments/environment';
import { Router } from '@angular/router';
import { throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { UserInfo } from '@app/shared/Model/UserInfo';
@Component({
  selector: 'app-my-info',
  templateUrl: './my-info.component.html',
  styleUrls: ['./my-info.component.css']
})
export class MyInfoComponent implements OnInit {
  public loginToken: string;
  user: UserInfo;
  constructor(private http: HttpClient, private router: Router) {
  }
  ngOnInit() {
    this.user = JSON.parse(window.localStorage.getItem('userinfo'))
    //console.log(this.user)
  }
  loginout() {
    window.localStorage.removeItem('userinfo');
    console.log("loginout Ok")
    this.router.navigate(['/']);
  }

}
