import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '@environments/environment';
import { Article } from '@app/shared/interface/article.interface';
import { FormGroup, FormControl, ValidationErrors, FormBuilder, Validators } from '@angular/forms';
import { Observer, Observable } from 'rxjs';
import { ManageBlogService } from '@app/shared/service/manage-blog.service';
import { ArticleService } from '@app/shared/service/article.service';
import { Httpresult } from '@app/shared/interface/httpresult.interface';
import { StateCode } from '@app/shared/enum/statecode.enum';
import { NzMessageService } from 'ng-zorro-antd';

@Component({
  selector: 'app-edit-blog',
  templateUrl: './edit-blog.component.html',
  styleUrls: ['./edit-blog.component.css']
})
export class EditBlogComponent implements OnInit {

  aid: string;
  article: Article;

  ngOnInit() {
  }
  editForm: FormGroup;

  submitForm(value: any): void {
    for (const key in this.editForm.controls) {
      this.editForm.controls[key].markAsDirty();
      this.editForm.controls[key].updateValueAndValidity();
    }
    this.manageblog.updateArticle<Httpresult>(value).subscribe(res => {
      console.log(res);
      if(res.code==StateCode.ok){
        this.msg.info(res.msg);
        this.editForm.reset();
        this.router.navigate(['/admin/blog']);
      }
    });
    //console.log(value);
    
    //this.msg.info("")
  }


  constructor(private fb: FormBuilder, 
    private route: ActivatedRoute, 
    private http: HttpClient, 
    private art: ArticleService, 
    private manageblog: ManageBlogService,
    private msg: NzMessageService,
    private router:Router) {
    this.route.paramMap.subscribe(params => {
      console.log(params.get('id'));
      this.editForm = this.fb.group({
        Id: ['', [Validators.required]],
        BlogTitle: ['', [Validators.required]],
        PicUrl: ['', [Validators.required]],
        PicIntroduce: ['', [Validators.required]],
        BlogContent: ['', [Validators.required]],
      });
      this.art.getArtcileDetail<Httpresult<Article>>(params.get('id')).subscribe(result => {

        if (result.code == StateCode.ok) {
          this.article = result.result;
          //console.log(this.article)

          this.aid = result.result.content;
          this.editForm = this.fb.group({
            Id: [result.result.id, [Validators.required]],
            BlogTitle: [result.result.title, [Validators.required]],
            PicUrl: [result.result.picUrl, [Validators.required]],
            PicIntroduce: [result.result.picInfo, [Validators.required]],
            BlogContent: [result.result.content, [Validators.required]],
          });
        }
      }, error => console.error(error));
    });


  }
}
