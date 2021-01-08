import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientcorsoriginmodalComponent } from './clientcorsoriginmodal.component';

describe('ClientcorsoriginmodalComponent', () => {
  let component: ClientcorsoriginmodalComponent;
  let fixture: ComponentFixture<ClientcorsoriginmodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientcorsoriginmodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientcorsoriginmodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
