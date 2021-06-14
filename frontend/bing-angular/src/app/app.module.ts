import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { AppRoutingModule } from './app-routing.module';
import { NgZorroAntdModule ,NZ_ICONS} from 'ng-zorro-antd';
import { IconDefinition } from '@ant-design/icons-angular';
import * as AllIcons from '@ant-design/icons-angular/icons';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { HttpClientModule,HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ReactiveFormsModule,FormsModule} from '@angular/forms';
import { MyInfoComponent } from './my-info/my-info.component';
import { ArticleListComponent } from './article-list/article-list.component';
import { MarkdownModule } from 'ngx-markdown';
import { ArticleDetailComponent } from './article-detail/article-detail.component';
import { ChatPageComponent } from './chat-page/chat-page.component';
import { ChatRoomComponent } from './chat-room/chat-room.component';
import { RegisterComponent } from './register/register.component';
import { GlobalErrorHandlingService } from './shared/service/global-error-handling.service';
import { AboutComponent } from './about/about.component';
import { ThirdLoginComponent } from './third-login/third-login.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
const antDesignIcons = AllIcons as {
   [key: string]: IconDefinition;
 };
 const icons: IconDefinition[] = Object.keys(antDesignIcons).map(key => antDesignIcons[key])
 
@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      LoginComponent,
      MyInfoComponent,
      ArticleListComponent,
      ArticleDetailComponent,
      ChatPageComponent,
      ChatRoomComponent,
      RegisterComponent,
      AboutComponent,
      ThirdLoginComponent,
      LandingPageComponent
   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      NgbModule,
      ReactiveFormsModule,
      FormsModule,
      DragDropModule,
      ScrollingModule,
      NgZorroAntdModule,
      BrowserAnimationsModule,
      MarkdownModule.forRoot({loader:HttpClient})
   ],
   providers: [{ provide: NZ_ICONS, useValue: icons } ,{provide: ErrorHandler, useClass: GlobalErrorHandlingService}],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
