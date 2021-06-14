import { Component } from '@angular/core';
import { SingleService } from './shared/service/single.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  isCollapsed = false;
  title = 'bing-angular';
  constructor(public single:SingleService){
    
  }
}
