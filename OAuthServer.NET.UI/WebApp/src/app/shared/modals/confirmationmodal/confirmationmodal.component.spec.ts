import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmationmodalComponent } from './confirmationmodal.component';

describe('ConfirmationmodalComponent', () => {
  let component: ConfirmationmodalComponent;
  let fixture: ComponentFixture<ConfirmationmodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmationmodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmationmodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
