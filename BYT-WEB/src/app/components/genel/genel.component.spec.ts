/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { GenelComponent } from './genel.component';

describe('GenelComponent', () => {
  let component: GenelComponent;
  let fixture: ComponentFixture<GenelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
