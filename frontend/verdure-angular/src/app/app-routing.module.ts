import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BlogListComponent } from './blog-list/blog-list.component';
import { BlogDetailComponent } from './blog-detail/blog-detail.component';
import {NavBoardComponent} from './nav-board/nav-board.component';
import {RootNavBarComponent} from './root-nav-bar/root-nav-bar.component';
import { from } from 'rxjs';
const routes: Routes = [
  {path:'blog-list',component:BlogListComponent},
  {path:'blog-detail/:bId',component:BlogDetailComponent},
  {path:'root-nav',component:RootNavBarComponent},
  {path:'dashboard',component:NavBoardComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
