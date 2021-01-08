import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientcreatemodalComponent } from './clientcreatemodal.component';

describe('ClientcreatemodalComponent', () => {
  let component: ClientcreatemodalComponent;
  let fixture: ComponentFixture<ClientcreatemodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientcreatemodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientcreatemodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
