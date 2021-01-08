import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientredirecturimodalComponent } from './clientredirecturimodal.component';

describe('ClientredirecturimodalComponent', () => {
  let component: ClientredirecturimodalComponent;
  let fixture: ComponentFixture<ClientredirecturimodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientredirecturimodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientredirecturimodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
