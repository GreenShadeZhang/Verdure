import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';
import { SingleService } from '@app/shared/service/single.service';
import { error } from 'protractor';
import { NzMessageService } from 'ng-zorro-antd';

@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.css']
})
export class LandingPageComponent implements OnInit {

  constructor(private route: ActivatedRoute,
    private httpclient: HttpClient,
    private single: SingleService,
    private router: Router,
    private msg: NzMessageService) { }

  ngOnInit() {
    this.route.queryParams.subscribe((params: any) => {
      console.log(params)
      console.log(params.code);
      console.log(params.state)
      var data = { "code": params.code, "ThirdType": 1 };
      this.httpclient.post<any>(environment.baseUrls.server + "account/third_login", data).subscribe(result => {
        if (result.jwtToken === null) {
          console.log("login-error");
          window.localStorage.removeItem('userinfo');
          this.msg.info("授权异常请重新登录。");
          this.router.navigate(['/third-login']);
        }
        else {
          window.localStorage.setItem('userinfo', JSON.stringify(result));
          console.log('save ok');
          this.single.newCpuValue$.next(result.userId);
          console.log(result.userId);
          this.router.navigate(['/']);
        }
      }, error => {
        console.log(error)
        this.msg.info("授权异常请重新登录。");
        this.router.navigate(['/third-login']);
      })
    });
  }

}
