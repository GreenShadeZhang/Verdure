import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BlogComponent } from './blog/blog.component';
import { EditBlogComponent } from './edit-blog/edit-blog.component';
import { AddBlogComponent } from './add-blog/add-blog.component';
import { WnsPushComponent } from './wns-push/wns-push.component';



const routes: Routes = [
 
  {path:'blog', component: BlogComponent},
  {path:'edit/:id', component: EditBlogComponent},
  {path:'add', component: AddBlogComponent},
  {path:'wns', component: WnsPushComponent},
  {path:'', component: BlogComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
