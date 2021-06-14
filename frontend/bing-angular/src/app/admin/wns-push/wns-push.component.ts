import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { SingleService } from '@app/shared/service/single.service';
import { environment } from '@environments/environment';
import { HttpRequest } from '@microsoft/signalr';
import { NzMessageService } from 'ng-zorro-antd';
import { Httpresult } from '@app/shared/interface/httpresult.interface';
import { StateCode } from '@app/shared/enum/statecode.enum';

@Component({
  selector: 'app-wns-push',
  templateUrl: './wns-push.component.html',
  styleUrls: ['./wns-push.component.css']
})
export class WnsPushComponent implements OnInit {
  username: string;
  password: string;
  rou: Router;
  constructor(private http: HttpClient,
    private router: Router,
    private fb: FormBuilder,
    private nzMsg: NzMessageService) {
  }
  validateForm: FormGroup;
  submitForm(value: any): void {
    for (const i in this.validateForm.controls) {
      this.validateForm.controls[i].markAsDirty();
      this.validateForm.controls[i].updateValueAndValidity();
    }
    this.http.post<Httpresult>(environment.baseUrls.server + environment.baseUrls.apiUrl + "applet/WnsChannel", value).subscribe(result => {
      result.code == StateCode.ok ? this.nzMsg.success(result.msg) : this.nzMsg.error(result.msg);
      this.validateForm.reset();
    });
  }

  ngOnInit() {
    this.validateForm = this.fb.group({
      AppName: [null, [Validators.required]],
      MsgContentXml: [null, [Validators.required]]
    });
  }

}
