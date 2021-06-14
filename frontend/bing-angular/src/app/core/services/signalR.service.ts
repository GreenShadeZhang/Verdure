import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '@environments/environment';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel
} from '@microsoft/signalr';
import { BehaviorSubject, Subject, Observable } from 'rxjs';
import { UserInfo } from '@app/shared/Model/UserInfo';
import { Msg } from '@app/shared/interface/msg.interface';
import { NzMessageService } from 'ng-zorro-antd';

const WAIT_UNTIL_ASPNETCORE_IS_READY_DELAY_IN_MS = 2000;

@Injectable({ providedIn: 'root' })
export class SignalRService {
  foodchanged$ = new Subject();
  messageReceived$ = new Subject<string>();
  toolTip$ = new Subject<string>();
  joinRoomUser$ = new Subject<string>();
  grouprecv$ = new Subject<Msg>();
  connectionEstablished$ = new BehaviorSubject<boolean>(false);
  private hubConnection: HubConnection;
  private loginToken: string;
  private roomId: string;
  userLocal: UserInfo;
  constructor(private router: Router,
    private msg: NzMessageService) {
  }

  sendChatMessage(groupName: string, mediatype: number, message: string) {
    console.log("enter")
    this.hubConnection.invoke('SendToGroup', groupName, mediatype, message);
  }

  joinChatGroup(groupName: string) {
    this.hubConnection.invoke('JoinGroup', groupName);
    console.log(groupName);
  }

  leaveChatGroup(groupName: string) {
    this.hubConnection.invoke('LeaveGroup', groupName, "gil");
    console.log(this.hubConnection.connectionId);
  }

  createConnection() {
    this.userLocal = JSON.parse(window.localStorage.getItem('userinfo'))
    this.userLocal==null?this.loginToken="":this.loginToken=this.userLocal.jwtToken;
    //this.loginToken = this.userLocal.jwtToken;
    console.log("this local token" + this.loginToken)
    this.roomId = window.localStorage.getItem('room_id');
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(environment.baseUrls.server + 'chathub?room_id=' + this.roomId, { accessTokenFactory: () => this.loginToken })
      .configureLogging(LogLevel.Information)
      .build();
  }

  startConnection() {
    if (this.hubConnection.state === HubConnectionState.Connected) {
      return;
    }
    setTimeout(() => {
      this.hubConnection.start().then(
        () => {
          console.log('Hub connection started');
          this.connectionEstablished$.next(true);
          console.log(this.hubConnection.connectionId)
          this.joinChatGroup(window.localStorage.getItem('room_id'));
        },
        error =>{
          console.log(error)
          // this.msg.error("请您登录以后再操作");
          // this.router.navigate(['/third-login']);
        } 
      );
    }, WAIT_UNTIL_ASPNETCORE_IS_READY_DELAY_IN_MS);
  }
closeConnection(){
  this.hubConnection.stop();
}
  registerOnServerEvents(): void {
    this.hubConnection.on('Send', (data: any) => {
      console.log(data)
      this.messageReceived$.next(data);
    });
    this.hubConnection.on('Connect', (data: any) => {
      this.toolTip$.next(data);
    });
    this.hubConnection.on('GroupSend', (data: any) => {
      console.log(data)
      this.joinRoomUser$.next(data);
    });
    this.hubConnection.on('GroupRecv', (data: Msg) => {
      console.log("msg" + data)
      this.grouprecv$.next(data);
    });
  }
}
