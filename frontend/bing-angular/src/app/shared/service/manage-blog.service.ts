import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Articlelist } from '../interface/Articlelist.interface';
import { environment } from '@environments/environment';
import { UserInfo } from '../Model/UserInfo';

@Injectable({
  providedIn: 'root'
})
export class ManageBlogService {
  public loginToken: string;
  user: UserInfo;
  constructor(private http:HttpClient) {
    this.user = JSON.parse(window.localStorage.getItem('userinfo'))
    this.user==null?this.loginToken="":this.loginToken=this.user.jwtToken;
   }
  getArticles<T>(status:number,pi:number,ps:number)
  {
    const header = new HttpHeaders({
      "Authorization": `Bearer ${this.loginToken}`
    });
   return this.http.get<T>(environment.baseUrls.server+environment.baseUrls.apiUrl+'manage/arts?status='+status+'&pi='+pi+'&ps='+ps,{ headers:header });
  }
  
  updateArticle<T>(art:any){
    const header = new HttpHeaders({
      "Authorization": `Bearer ${this.loginToken}`
    });
   return this.http.post<T>(environment.baseUrls.server+environment.baseUrls.apiUrl+'manage/update_article',art,{ headers: header })
  }
  
  changeArticleStatus(art:any){
    const header = new HttpHeaders({
      "Authorization": `Bearer ${this.loginToken}`
    });
    console.log(art)
   return this.http.post<any>(environment.baseUrls.server+environment.baseUrls.apiUrl+'manage/change_article_status',art,{ headers: header })
  }

  changeArticleType(art:any){
    const header = new HttpHeaders({
      "Authorization": `Bearer ${this.loginToken}`
    });
    console.log(art)
   return this.http.post<any>(environment.baseUrls.server+environment.baseUrls.apiUrl+'manage/change_article_type',art,{ headers: header })
  }
}
