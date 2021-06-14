import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '@environments/environment';
import { from, Observable, Subject } from 'rxjs';
import { Article } from '../interface/article.interface';
@Injectable({
  providedIn: 'root'
})
export class ArticleService {
constructor(private http:HttpClient) { }

getArticles<T>(type:number,pi:number,ps:number)
{
 return this.http.get<T>(environment.baseUrls.server+environment.baseUrls.apiUrl+'Articles/arts?type='+type+'&pi='+pi+'&ps='+ps);
}

getArtcileDetail<T>(id:string){
  return this.http.get<T>(environment.baseUrls.server+environment.baseUrls.apiUrl+'Articles/article_detail?id='+id);
}
}