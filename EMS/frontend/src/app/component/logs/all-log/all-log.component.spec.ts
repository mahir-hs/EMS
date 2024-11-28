import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllLogComponent } from './all-log.component';

describe('AllLogComponent', () => {
  let component: AllLogComponent;
  let fixture: ComponentFixture<AllLogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllLogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllLogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
