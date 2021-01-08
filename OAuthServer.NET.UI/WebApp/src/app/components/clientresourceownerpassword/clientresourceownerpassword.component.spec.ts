import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientresourceownerpasswordComponent } from './clientresourceownerpassword.component';

describe('ClientresourceownerpasswordComponent', () => {
  let component: ClientresourceownerpasswordComponent;
  let fixture: ComponentFixture<ClientresourceownerpasswordComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientresourceownerpasswordComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientresourceownerpasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
