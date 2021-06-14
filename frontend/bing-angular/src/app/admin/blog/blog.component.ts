import { Component, OnInit } from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Observable } from 'rxjs';
import { Articlelist } from '@app/shared/interface/Articlelist.interface';
import { Article } from '@app/shared/interface/article.interface';
import { ManageBlogService } from '@app/shared/service/manage-blog.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-blog',
  templateUrl: './blog.component.html',
  styleUrls: ['./blog.component.css']
})
export class BlogComponent implements OnInit {
  articlelist$: Observable<Articlelist>;
  arts: Article[];
  pageIndex: number = 1;
  pageSize: number = 10;
  total: number = 10;
  constructor(public msg: NzMessageService, private manageblog: ManageBlogService,private router:Router) {
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
  ChangeBlogStatus(id: string, status: number) {
    var data = { "id": id, "status": status };
    this.manageblog.changeArticleStatus(data).subscribe(result => {
      this.getBlogList();
    });
  }
  ChangeBlogType(id: string, type: number) {
    var data = { "id": id, "type": type };
    this.manageblog.changeArticleType(data).subscribe(result => {
      this.articlelist$ = this.manageblog.getArticles<Articlelist>(0, this.pageIndex, this.pageSize);
      this.getBlogList();
    });
  }
  getBlogList() {
    this.manageblog.getArticles<Articlelist>(0, this.pageIndex, this.pageSize).subscribe(result => {
      this.total = result.pageTotal;
      this.arts = result.arts;
    }, error => {
      console.log(error.status)
      this.msg.error("请先登录再操作");
      this.router.navigate(['/login']);
      window.localStorage.removeItem('userinfo');
    });
  }
}
