import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { BlogComponent } from './blog/blog.component';
import { EditBlogComponent } from './edit-blog/edit-blog.component';
import { AddBlogComponent } from './add-blog/add-blog.component';

import { ReactiveFormsModule,FormsModule} from '@angular/forms';



import { NgZorroAntdModule ,NZ_ICONS} from 'ng-zorro-antd';
import { IconDefinition } from '@ant-design/icons-angular';
import * as AllIcons from '@ant-design/icons-angular/icons';
import { WnsPushComponent } from './wns-push/wns-push.component';
const antDesignIcons = AllIcons as {
  [key: string]: IconDefinition;
};
const icons: IconDefinition[] = Object.keys(antDesignIcons).map(key => antDesignIcons[key])
@NgModule({
  declarations: [
    BlogComponent,
    EditBlogComponent,
    AddBlogComponent,
    WnsPushComponent
  ],
  imports: [
    CommonModule, 
    NgZorroAntdModule,
    ReactiveFormsModule,
    FormsModule,
    AdminRoutingModule,
  ]
})
export class AdminModule { }
