import { Component, OnInit } from '@angular/core';
import { UserInfo } from '@app/shared/Model/UserInfo';
import { HttpClient, HttpHeaders, HttpRequest, HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '@environments/environment';
import { NzMessageService, UploadFile } from 'ng-zorro-antd';
import { filter } from 'rxjs/operators';
import { Httpresult } from '@app/shared/interface/httpresult.interface';
import { StateCode } from '@app/shared/enum/statecode.enum';
import { error } from 'protractor';

@Component({
  selector: 'app-add-blog',
  templateUrl: './add-blog.component.html',
  styleUrls: ['./add-blog.component.css']
})
export class AddBlogComponent implements OnInit {

  public loginToken: string;
  user: UserInfo;
  title:string;
  pic_url:string;
  pic_info:string;
  constructor(private http: HttpClient, 
    private router: Router, 
    private msg: NzMessageService) {
  }
  ngOnInit() {
    this.user = JSON.parse(window.localStorage.getItem('userinfo'))
    console.log(this.user)
  }

  uploading = false;
  fileList: UploadFile[] = [];
  beforeUpload = (file: UploadFile): boolean => {
    console.log(file)
    console.log(this.title)
    console.log(file.name)
    this.title=file.name
    this.fileList = this.fileList.concat(file);
    return false;
  };
  handleUpload(): void {
    const formData = new FormData();
    // tslint:disable-next-line:no-any
    this.fileList.forEach((file: any) => {
      formData.append('id', file);
      formData.append('title',this.title);
      formData.append('pic_url',this.pic_url);
      formData.append('pic_info',this.pic_info);
    });
    this.uploading = true;
    // You can use any AJAX library you like

    this.loginToken = this.user.jwtToken;
    const header = new HttpHeaders({
      "Authorization": `Bearer ${this.loginToken}`
    });
    this.http.post<Httpresult>(environment.baseUrls.server + environment.baseUrls.apiUrl + "manage/import_article", formData, { headers: header })
      .subscribe(result => {
        if(result.code==StateCode.ok){
          this.uploading = false;
          this.fileList = [];
          this.msg.success(result.msg);
          this.router.navigate(['/admin/blog']);
        }
        
      },error=>{
        console.log(error)
        this.msg.error("导入失败。");
        this.uploading = false;
        this.fileList = [];
      });
  }
}
