import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {HomeComponent} from './home/home.component'
import {LoginComponent} from './login/login.component';
import {MyInfoComponent} from './my-info/my-info.component';
import {ArticleListComponent} from './article-list/article-list.component';
import { ArticleDetailComponent} from './article-detail/article-detail.component';
import{ChatPageComponent} from './chat-page/chat-page.component';
import{ChatRoomComponent} from './chat-room/chat-room.component';
import{RegisterComponent} from './register/register.component';
import { from } from 'rxjs';
import { AboutComponent } from './about/about.component';
import { ThirdLoginComponent } from './third-login/third-login.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
const routes: Routes = [
  {path:'admin',loadChildren: () => import('./admin/admin.module').then(mod => mod.AdminModule)},
  {path:'',component:HomeComponent,pathMatch: 'full'},
  {path:'login',component:LoginComponent},
  {path:'register',component:RegisterComponent},
  {path:'myinfo',component:MyInfoComponent},
  {path:'alist',component:ArticleListComponent},
  {path:'chat',component:ChatPageComponent},
  {path:'alist/:aid',component:ArticleDetailComponent},
  {path:'chatroom',component:ChatRoomComponent},
  {path:'about',component:AboutComponent},
  {path:'third-login',component:ThirdLoginComponent},
  {path:'signin-qq',component:LandingPageComponent},
  
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
