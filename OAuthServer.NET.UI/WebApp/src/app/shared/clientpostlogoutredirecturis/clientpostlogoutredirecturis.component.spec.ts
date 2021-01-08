import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientpostlogoutredirecturisComponent } from './clientpostlogoutredirecturis.component';

describe('ClientpostlogoutredirecturisComponent', () => {
  let component: ClientpostlogoutredirecturisComponent;
  let fixture: ComponentFixture<ClientpostlogoutredirecturisComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientpostlogoutredirecturisComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientpostlogoutredirecturisComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
