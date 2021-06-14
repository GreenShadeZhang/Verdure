import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';
import { Article } from '@app/shared/interface/article.interface';
import { ArticleService } from '@app/shared/service/article.service';
import { Httpresult } from '@app/shared/interface/httpresult.interface';
import { StateCode } from '@app/shared/enum/statecode.enum';
import { NzMessageService } from 'ng-zorro-antd';
@Component({
  selector: 'app-article-detail',
  templateUrl: './article-detail.component.html',
  styleUrls: ['./article-detail.component.css']
})
export class ArticleDetailComponent implements OnInit {
  blogContent: string;
  constructor(private route: ActivatedRoute,
    private article: ArticleService,
    private nzmessage: NzMessageService) {

  }
  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      console.log(params.get('aid'));
      this.article.getArtcileDetail<Httpresult<Article>>(params.get('aid')).subscribe(result => {
        if (result.code == StateCode.ok) {
          this.blogContent = result.result.content;
        }
        else if (result.code == StateCode.nocontent) {
          this.nzmessage.info(result.msg);
        }
      }, error => console.error(error));
    });
  }

}
