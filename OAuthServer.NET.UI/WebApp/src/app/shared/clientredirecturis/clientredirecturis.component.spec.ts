import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientredirecturisComponent } from './clientredirecturis.component';

describe('ClientredirecturisComponent', () => {
  let component: ClientredirecturisComponent;
  let fixture: ComponentFixture<ClientredirecturisComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientredirecturisComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientredirecturisComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
