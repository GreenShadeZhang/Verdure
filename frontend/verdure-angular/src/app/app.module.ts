import { NgModule } from '@angular/core';
import { HttpClientModule,HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { MatSliderModule } from '@angular/material/slider';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RootNavComponent } from './root-nav/root-nav.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import { BlogListComponent } from './blog-list/blog-list.component';
import {MatListModule} from '@angular/material/list';
import {MatCardModule} from '@angular/material/card';
import { MarkdownModule } from 'ngx-markdown';
import { BlogDetailComponent } from './blog-detail/blog-detail.component';
@NgModule({
  declarations: [					
    AppComponent,
      RootNavComponent,
      BlogListComponent,
      BlogDetailComponent
   ],
  imports: [
    MatSliderModule,
    MatToolbarModule,
    MatButtonModule,
    MatCardModule,
    MatListModule,
    MatIconModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MarkdownModule.forRoot({loader:HttpClient})
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
