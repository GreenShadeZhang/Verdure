import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SignalRService } from '@app/core/services/signalR.service';
import { Observable, forkJoin } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '@environments/environment';
import { Chatroom } from '@app/shared/interface/chatroom.interface';
@Component({
  selector: 'app-chat-page',
  templateUrl: './chat-page.component.html',
  styleUrls: ['./chat-page.component.css']
})
export class ChatPageComponent implements OnInit {
  grouprecv$: Observable<string>;
  constructor(private http: HttpClient, private router: Router) {
  }
  groupName: string;
  groupList: Chatroom[];
  ngOnInit() {
    this.http.get<Chatroom[]>(environment.baseUrls.server + environment.baseUrls.apiUrl + 'ChatGroups').subscribe(result => {
      console.log(result)
      this.groupList = result;
    }, error => console.error(error));
  }
  onSubmit() {
    var data = { "Title": this.groupName };
    console.log(data)
    this.http.post<Chatroom>(environment.baseUrls.server + environment.baseUrls.apiUrl + "ChatGroups", data).subscribe(result => {
      console.log(result);
    });
  }

  joingroup(group: Chatroom): void {
    console.log(group.id)
    window.localStorage.setItem('room_id', group.id);
    this.router.navigate(['/chatroom']);
  }
}
