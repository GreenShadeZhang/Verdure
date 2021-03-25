import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
@Component({
  selector: 'app-blog-detail',
  templateUrl: './blog-detail.component.html',
  styleUrls: ['./blog-detail.component.css']
})
export class BlogDetailComponent implements OnInit {
  blogContent: string | undefined;
  constructor(private route: ActivatedRoute,private http:HttpClient) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      console.log(params.get('bId'));
      this.http.get("../../assets/testdoc.md",{responseType:"text"}).subscribe(result=>{
        //console.log(result)
        this.blogContent = result;
      })
    });
  }

}
