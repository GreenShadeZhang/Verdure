
import { ChangeDetectionStrategy, Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SignalRService } from '@app/core/services/signalR.service';
import { BehaviorSubject, Subscription, Observable, forkJoin } from 'rxjs';
import { Router, NavigationEnd, NavigationStart } from '@angular/router';
import { environment } from '@environments/environment';
import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';
import { Chatmsg } from '@app/shared/interface/chatmsg.interface';
import { ChatmsgService } from '@app/shared/service/Chatmsg.service';
import { Msg } from '@app/shared/interface/msg.interface';
import { CdkScrollable, ScrollDispatcher, ViewportRuler } from '@angular/cdk/overlay';
import { UserInfo } from '@app/shared/Model/UserInfo';
import { NzMessageService } from 'ng-zorro-antd';
import { error } from 'protractor';
import { Httpresult } from '@app/shared/interface/httpresult.interface';
@Component({
  selector: 'app-chat-room',
  templateUrl: './chat-room.component.html',
  styleUrls: ['./chat-room.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class ChatRoomComponent implements OnInit {
  @ViewChild(CdkVirtualScrollViewport, { static: true }) virtualScroll: CdkVirtualScrollViewport;
  toolTip$: Observable<string>;
  joinRoomUser$: Observable<string>;
  grouprecv$: Observable<string>;
  ds: MyDataSource;
  user: UserInfo;
  constructor(private http: HttpClient, private readonly signalRService: SignalRService, private chat: ChatmsgService, router: Router,
    private nzmessage: NzMessageService) {
      this.user=chat.user;
    this.signalRService.createConnection();
    this.signalRService.registerOnServerEvents();
    this.signalRService.startConnection();
    this.ds = new MyDataSource(chat,nzmessage,router);
    router.events.subscribe((val) => {
      // see also 
      //console.log(val);
      if(val instanceof NavigationStart){
        console.log("开始导航")
        this.signalRService.closeConnection();
      }
      //console.log(val instanceof NavigationStart) 
  });
  }
  sendmsg: string;
  flag: number = 0;
  oldsize: number;
  ngOnInit() {
    this.toolTip$ = this.signalRService.toolTip$;
    console.log("room_id" + this.toolTip$)
    this.joinRoomUser$ = this.signalRService.joinRoomUser$;
    this.joinRoomUser$.subscribe(join=>{
      this.nzmessage.info(join);
    });
    this.signalRService.grouprecv$.subscribe(message => {
      console.log(message)
      if(message!=null&&message.userId==this.chat.getUserId())
      {
        message.isMe=true;
      }
      console.log(message)
      this.ds.AddChatData(message)
      console.log("vs detail" + this.virtualScroll)


    });
  }
  onsendmsg() {
    //this.virtualScroll.scrollToOffset(500 * 73);
    console.log("bengin send msg")
    console.log(this.sendmsg)
    if(this.sendmsg!=null&&this.sendmsg!=""){
      this.signalRService.sendChatMessage(window.localStorage.getItem('room_id'),0, this.sendmsg);
    }
    else{
      this.nzmessage.warning("请输入消息");
    }
    
  }

  nextBatch(e: number) {
    //this.ds.AddData(e, this.flag);
    console.log("data lengh" + this.virtualScroll.getDataLength()+"scroll "+e)
    if (this.flag == 0) {

      this.virtualScroll.scrollToOffset(this.virtualScroll.getDataLength() * 73);
      this.oldsize = this.virtualScroll.measureScrollOffset("top");
    }
    // else {
    //   let newsize = this.virtualScroll.measureScrollOffset("top")
    //   console.log("top"+newsize)
    //   console.log("old"+this.oldsize)
    //   if ((newsize - this.oldsize)<0) {
    //     //this.ds.AddMsg(1);
    //     // console.log(newsize - this.oldsize)
    //     // console.log("vs " + e)
        
    //   }
    //   this.oldsize = newsize;
    // }
    this.flag = 25;
  }
}
class MyDataSource extends DataSource<Msg> {
  private length = 20;
  private pageSize = 20;
  private flag = 1;
  private cachedData = Array.from<Msg>({ length: this.length });
  private fetchedPages = new Set<number>();
  public dataStream = new BehaviorSubject<Msg[]>(this.cachedData);
  private subscription = new Subscription();

  constructor(private chatMsg: ChatmsgService,private nzmsg:NzMessageService,private router: Router) {
    super();
  }

  connect(collectionViewer: CollectionViewer): Observable<Msg[]> {
    this.subscription.add(
      collectionViewer.viewChange.subscribe(range => {
        const startPage = this.getPageForIndex(range.start);
        const endPage = this.getPageForIndex(range.end);
        console.log("start range:" + range.start + "end range:" + range.end)
        //console.log("start page:" + startPage + "end page:" + endPage+"last page"+this.lastPage)
        if (this.flag == 1) {
          this.fetchPage(1);
          this.flag = 3;
        }
        // for (let i = startPage; i <= endPage; i++) {
        //   //console.log("index i:" + i)
        //   this.fetchPage(i);
        // }
      })
    );
    return this.dataStream;
  }

  disconnect(): void {
    this.subscription.unsubscribe();
  }

  private getPageForIndex(index: number): number {
    return Math.floor(index / this.pageSize);
  }

  AddData(pi: number, flag: number): void {
    // this.fetchedPages.add(this.getPageForIndex(pi));
    // if (this.fetchedPages.has(this.getPageForIndex(pi))) {
    //   return;
    // }

    if (this.getPageForIndex(pi) == 0 && flag != 0) {
      this.chatMsg.getChatMsgs<Httpresult<Chatmsg>>(window.localStorage.getItem('room_id'),0, 2, 20).subscribe(res => {
        console.log(res)
        this.cachedData.push(...res.result.msgs)
        this.dataStream.next(this.cachedData);
      })
    }

  }
AddChatData(msg:Msg):void{
  this.cachedData.push(msg)
  this.dataStream.next(this.cachedData);
}
  AddMsg(pi: number): void {
    // this.fetchedPages.add(this.getPageForIndex(pi));
    // if (this.fetchedPages.has(this.getPageForIndex(pi))) {
    //   return;
    // }

    this.chatMsg.getChatMsgs<Httpresult<Chatmsg>>(window.localStorage.getItem('room_id'),0, 1, 20).subscribe(res => {
      console.log(res)
      this.cachedData.splice(0, 0, ...res.result.msgs)
      this.dataStream.next(this.cachedData);
    })
  }
  public fetchPage(page: number): void {
    //console.log("login fetch page" + page)
    if (this.fetchedPages.has(page)) {
      return;
    }
    //console.log("get data begin" + page)
    this.fetchedPages.add(page);
    this.chatMsg.getChatMsgs<Httpresult<Chatmsg>>(window.localStorage.getItem('room_id'),0, 1, 20).subscribe(res => {
      console.log(res)
      this.cachedData.splice(0, res.result.pageTotal, ...res.result.msgs.reverse())
      this.dataStream.next(this.cachedData);
    },error=>{
      console.error(error)
      this.nzmsg.error("请您登录以后再操作");
      this.router.navigate(['/third-login']);
      window.localStorage.removeItem('userinfo');
    })
  }
}