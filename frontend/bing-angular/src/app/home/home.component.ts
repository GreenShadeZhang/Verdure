
import { environment } from '@environments/environment';
import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { BehaviorSubject, Subject, Observable } from 'rxjs';
import { Articlelist } from '@app/shared/interface/Articlelist.interface';
import { Article } from '@app/shared/interface/article.interface';
import { Banner } from '@app/shared/interface/banner.interface';
import { Httpresult } from '@app/shared/interface/httpresult.interface';
import { CommonService } from '@app/shared/service/common.service';
import { ArticleService } from '@app/shared/service/article.service';
import { StateCode } from '@app/shared/enum/statecode.enum';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  articlelist$: Observable<Httpresult<Articlelist>>;
  arts: Article[];
  pageIndex = 1;
  pageSize = 6;
  total = 10;
  array = [1, 2, 3, 4];
  public banner: Banner[];
  constructor(private article: ArticleService,
    private common: CommonService) {
  }

  ngOnInit() {
    this.common.getBingWallpaper<Httpresult<Banner[]>>().subscribe(res => {
      if (res.code == StateCode.ok) {
        this.banner = res.result;
      }
    }, error => console.error(error));
    this.getTopBlogList();
  }

  changePageIndex(pageIndex: number) {
    this.pageIndex = pageIndex;
    this.getTopBlogList();

  }
  PageSize(pageSize: number) {
    this.pageSize = pageSize;
    this.getTopBlogList();
  }
  getTopBlogList() {
    this.article.getArticles<Httpresult<Articlelist>>(1, this.pageIndex, this.pageSize).subscribe(result => {
      if (result.code === StateCode.ok) {
        this.total = result.result.pageTotal;
        this.arts = result.result.arts;
      }
    }, error => console.error(error));
  }
}
