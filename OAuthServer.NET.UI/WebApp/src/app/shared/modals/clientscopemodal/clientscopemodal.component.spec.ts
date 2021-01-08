import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientscopemodalComponent } from './clientscopemodal.component';

describe('ClientscopemodalComponent', () => {
  let component: ClientscopemodalComponent;
  let fixture: ComponentFixture<ClientscopemodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientscopemodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientscopemodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
