import { Component, Input } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AbstractControl, FormBuilder, FormGroup } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { TextInputPipe } from '../../pipes/text-input.pipe';
import { LoggerService } from '../../services/logger.service';
import { NotifyService } from '../../services/notify.service';
import { LanguageService } from '../../services/language.service';
import { ReportFormComponent } from './report-form.component';
import ReportRequest from '../../models/report-request';

describe('ReportFormComponent', () => {
  const testName = 'testName';
  const testAmount = 2;
  let component: ReportFormComponent;
  let fixture: ComponentFixture<ReportFormComponent>;
  let nameSearchControl: AbstractControl | null;
  let amountControl: AbstractControl | null;
  const loggerServiceSpy = jasmine.createSpyObj('LoggerService', ['info']);
  const notifyServiceSpy = jasmine.createSpyObj('NotifyService', ['error']);
  const languageServiceSpy = jasmine.createSpyObj('LanguageService', ['use']);

  beforeEach(async () => {
    @Component({ selector: 'form', template: '' })
    class FormComponentStub {
      @Input() formGroup: FormGroup | undefined;
    }

    @Component({ selector: 'mat-form-field', template: '' })
    class MatFormFieldComponentStub {}

    @Component({ selector: 'mat-error', template: '' })
    class MatErrorComponentStub {}

    @Component({ selector: 'mat-label', template: '' })
    class MatLabelComponentStub {}

    @Component({ selector: 'input', template: '' })
    class InputComponentStub {
      @Input() maxlength: string | number | null = null;
    }

    @Component({ selector: 'app-button', template: '' })
    class AppButtonComponentStub {
      @Input() disabled: boolean = false;
    }

    await TestBed.configureTestingModule({
      declarations: [
        ReportFormComponent,
        FormComponentStub,
        MatFormFieldComponentStub,
        InputComponentStub,
        MatErrorComponentStub,
        MatLabelComponentStub,
        AppButtonComponentStub,
      ],
      providers: [
        FormBuilder,
        TextInputPipe,
        { provide: LoggerService, useValue: loggerServiceSpy },
        { provide: NotifyService, useValue: notifyServiceSpy },
        { provide: LanguageService, useValue: languageServiceSpy },
      ],
      imports: [TranslateModule.forRoot()],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    nameSearchControl = component.form.get('nameSearch');
    amountControl = component.form.get('amount');
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });

  it('should create a form with 2 controls', () => {
    expect(component.form.contains('nameSearch')).toBeTruthy();
    expect(component.form.contains('amount')).toBeTruthy();
  });

  it('should has invalid name control when its max length is above the max length limit', () => {
    nameSearchControl?.setValue('a'.repeat(component.maxNameLength + 1));

    expect(nameSearchControl?.valid).toBeFalsy();
  });

  it('should has invalid name control when its max length is within the max length limit', () => {
    nameSearchControl?.setValue('a'.repeat(component.maxNameLength));

    expect(nameSearchControl?.valid).toBeTruthy();
  });

  it('should make the amount control required', () => {
    amountControl?.setValue('');

    expect(amountControl?.valid).toBeFalsy();
  });

  it('should has invalid amount control when its amount is above the max limit', () => {
    amountControl?.setValue(component.maxAmount + 1);

    expect(amountControl?.valid).toBeFalsy();
  });

  it('should has invalid amount control when its amount is within the max limit', () => {
    amountControl?.setValue(component.maxAmount);

    expect(amountControl?.valid).toBeTruthy();
  });

  it('should has invalid amount control when its amount is below the min limit', () => {
    amountControl?.setValue(component.minAmount - 1);

    expect(amountControl?.valid).toBeFalsy();
  });

  it('should has invalid amount control when its amount is within the min limit', () => {
    amountControl?.setValue(component.minAmount);

    expect(amountControl?.valid).toBeTruthy();
  });

  it('should raise onSubmit event when onFormSubmit called', () => {
    spyOn(component.onSubmit, 'emit');

    component.onFormSubmit();

    expect(component.onSubmit.emit).toHaveBeenCalled();
  });

  it('should raise event with correct data when the onFormSubmit called', () => {
    let request: ReportRequest = {
      name: '',
      amount: 0,
    };
    component.onSubmit.subscribe((r) => (request = r));
    nameSearchControl?.setValue(testName);
    amountControl?.setValue(testAmount);

    component.onFormSubmit();

    expect(request.name).toBe(testName.toLowerCase());
    expect(request.amount).toBe(testAmount);
  });

  it('should raise onCancel event with when the onReportCancel called and data is loading', () => {
    component.isDataLoading = true;
    spyOn(component.onCancel, 'emit');

    component.onReportCancel();

    expect(component.onCancel.emit).toHaveBeenCalled();
  });

  it('should raise onCancel event with when the onReportCancel called and data is not loading', () => {
    component.isDataLoading = false;
    spyOn(component.onCancel, 'emit');

    component.onReportCancel();

    expect(component.onCancel.emit).toHaveBeenCalledTimes(0);
  });
});
