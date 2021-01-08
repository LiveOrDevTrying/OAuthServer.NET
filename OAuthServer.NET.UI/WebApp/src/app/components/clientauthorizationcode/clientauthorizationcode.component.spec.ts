import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientauthorizationcodeComponent } from './clientauthorizationcode.component';

describe('ClientauthorizationcodeComponent', () => {
  let component: ClientauthorizationcodeComponent;
  let fixture: ComponentFixture<ClientauthorizationcodeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientauthorizationcodeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientauthorizationcodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
