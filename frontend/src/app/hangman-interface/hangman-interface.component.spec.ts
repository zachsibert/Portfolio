import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HangmanInterfaceComponent } from './hangman-interface.component';

describe('HangmanInterfaceComponent', () => {
  let component: HangmanInterfaceComponent;
  let fixture: ComponentFixture<HangmanInterfaceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HangmanInterfaceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HangmanInterfaceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
