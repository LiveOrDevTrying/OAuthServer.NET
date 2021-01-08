import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientpostlogoutredirecturimodalComponent } from './clientpostlogoutredirecturimodal.component';

describe('ClientpostlogoutredirecturimodalComponent', () => {
  let component: ClientpostlogoutredirecturimodalComponent;
  let fixture: ComponentFixture<ClientpostlogoutredirecturimodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientpostlogoutredirecturimodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientpostlogoutredirecturimodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
