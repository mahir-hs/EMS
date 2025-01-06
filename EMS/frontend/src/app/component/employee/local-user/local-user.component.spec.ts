import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LocalUserComponent } from './local-user.component';

describe('LocalUserComponent', () => {
  let component: LocalUserComponent;
  let fixture: ComponentFixture<LocalUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LocalUserComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LocalUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
