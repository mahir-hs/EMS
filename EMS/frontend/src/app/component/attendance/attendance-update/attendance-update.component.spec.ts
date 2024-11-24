import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AttendanceUpdateComponent } from './attendance-update.component';

describe('AttendanceUpdateComponent', () => {
  let component: AttendanceUpdateComponent;
  let fixture: ComponentFixture<AttendanceUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AttendanceUpdateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AttendanceUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
