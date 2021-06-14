import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '@environments/environment';
import { NgbDateStructAdapter } from '@ng-bootstrap/ng-bootstrap/datepicker/adapters/ngb-date-adapter';
import { from } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SingleService } from '@app/shared/service/single.service';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  username: string;
  password: string;
  httpclient: HttpClient;
  rou: Router;
  constructor(http: HttpClient, router: Router, private fb: FormBuilder, private single: SingleService) {
    this.httpclient = http;
    this.rou = router;

  }
  validateForm: FormGroup;

  submitForm(value: any): void {
    for (const i in this.validateForm.controls) {
      this.validateForm.controls[i].markAsDirty();
      this.validateForm.controls[i].updateValueAndValidity();
    }
    console.log("提交")
    console.log(value)
    this.httpclient.post<any>(environment.baseUrls.server + "account/login", value).subscribe(result => {
      console.log(result.token);
      window.localStorage.setItem('userinfo', JSON.stringify(result));
      console.log('save ok');
      this.single.newCpuValue$.next(result.userId);
      console.log(result.userId);
      this.rou.navigate(['/']);
    });
  }


  ngOnInit(): void {
    this.validateForm = this.fb.group({
      Email: [null, [Validators.required]],
      password: [null, [Validators.required]],
      remember: [true]
    });
  }
}
