import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientscopesComponent } from './clientscopes.component';

describe('ClientscopesComponent', () => {
  let component: ClientscopesComponent;
  let fixture: ComponentFixture<ClientscopesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientscopesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientscopesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
