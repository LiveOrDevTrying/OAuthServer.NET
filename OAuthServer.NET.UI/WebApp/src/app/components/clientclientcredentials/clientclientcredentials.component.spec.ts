import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientclientcredentialsComponent } from './clientclientcredentials.component';

describe('ClientclientcredentialsComponent', () => {
  let component: ClientclientcredentialsComponent;
  let fixture: ComponentFixture<ClientclientcredentialsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientclientcredentialsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientclientcredentialsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
