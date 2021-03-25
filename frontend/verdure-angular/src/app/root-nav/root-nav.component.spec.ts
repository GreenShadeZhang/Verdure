/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { RootNavComponent } from './root-nav.component';

describe('RootNavComponent', () => {
  let component: RootNavComponent;
  let fixture: ComponentFixture<RootNavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RootNavComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RootNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
