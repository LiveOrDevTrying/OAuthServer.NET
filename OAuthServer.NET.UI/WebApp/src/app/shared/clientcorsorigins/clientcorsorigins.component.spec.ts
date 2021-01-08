import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientcorsoriginsComponent } from './clientcorsorigins.component';

describe('ClientcorsoriginsComponent', () => {
  let component: ClientcorsoriginsComponent;
  let fixture: ComponentFixture<ClientcorsoriginsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientcorsoriginsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientcorsoriginsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
