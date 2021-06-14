import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { SingleService } from '@app/shared/service/single.service';
@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  isCollapsed = false;
  isExpanded = false;
  collapse() {
    this.isExpanded = false;
  }
  toggle() {
    this.isExpanded = !this.isExpanded;
  }
  constructor(private single: SingleService) {
  }
  ngOnInit() {
  }

}
