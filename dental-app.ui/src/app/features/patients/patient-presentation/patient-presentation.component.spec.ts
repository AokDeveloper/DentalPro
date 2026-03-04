import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PatientPresentationComponent } from './patient-presentation.component';

describe('PatientPresentationComponent', () => {
  let component: PatientPresentationComponent;
  let fixture: ComponentFixture<PatientPresentationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PatientPresentationComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PatientPresentationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
