import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Subject, Observable } from 'rxjs';
import { Articlelist } from '@app/shared/interface/Articlelist.interface';
import { Article } from '@app/shared/interface/article.interface';
import { Httpresult } from '@app/shared/interface/httpresult.interface';
import { ArticleService } from '@app/shared/service/article.service';
import { StateCode } from '@app/shared/enum/statecode.enum';
@Component({
  selector: 'app-article-list',
  templateUrl: './article-list.component.html',
  styleUrls: ['./article-list.component.css']
})
export class ArticleListComponent implements OnInit {
  articlelist$: Observable<Httpresult<Articlelist>>;
  arts: Article[];
  pageIndex: number = 1;
  pageSize: number = 5;
  total: number = 10;
  constructor(private article: ArticleService) {
  }

  ngOnInit() {
    this.getBlogList();
  }

  changePageIndex(pageIndex: number) {
    this.pageIndex = pageIndex;
    this.getBlogList();
  }
  PageSize(pageSize: number) {
    this.pageSize = pageSize;
    this.getBlogList();
  }
  getBlogList() {
    this.article.getArticles<Httpresult<Articlelist>>(0, this.pageIndex, this.pageSize).subscribe(result => {
      if (result.code === StateCode.ok) {
        this.total = result.result.pageTotal;
        this.arts = result.result.arts;
      }
    }, error => {
      console.log(error)
    }
    );
  }
}