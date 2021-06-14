/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { WnsPushComponent } from './wns-push.component';

describe('WnsPushComponent', () => {
  let component: WnsPushComponent;
  let fixture: ComponentFixture<WnsPushComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WnsPushComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WnsPushComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
