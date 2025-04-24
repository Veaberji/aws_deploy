import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { tap } from 'rxjs';
import { LoggerService } from '../../../../app/shared/services/logger.service';
import { TextInputPipe } from '../../pipes/text-input.pipe';
import ReportRequest from '../../models/report-request';

@UntilDestroy()
@Component({
  selector: 'app-report-form',
  templateUrl: './report-form.component.html',
  styleUrls: ['./report-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ReportFormComponent implements OnInit {
  form: FormGroup = new FormGroup({ name: new FormControl(), amount: new FormControl() });
  @Input() isDataLoading: boolean = false;
  @Input() maxNameLength: number = 0;
  @Input() minAmount: number = 0;
  @Input() maxAmount: number = 0;
  @Output() onSubmit = new EventEmitter<ReportRequest>();
  @Output() onCancel = new EventEmitter();

  constructor(private builder: FormBuilder, private inputPipe: TextInputPipe, private logger: LoggerService) {}

  ngOnInit(): void {
    this.initForm();
  }

  get nameSearch(): AbstractControl | null {
    return this.form.get('nameSearch');
  }

  get amount(): AbstractControl | null {
    return this.form.get('amount');
  }

  onFormSubmit(): void {
    const request: ReportRequest = {
      name: this.nameSearch?.value,
      amount: this.amount?.value,
    };
    this.logger.info('Report Requested', request);
    this.onSubmit.emit(request);
  }

  onReportCancel(): void {
    this.logger.info('Report Canceled');
    if (!this.isDataLoading) {
      return;
    }

    this.onCancel.emit();
  }

  onInputChanged(details: string, text: string): void {
    this.logger.info(`${details}: ${text}`);
  }

  private initForm() {
    this.form = this.builder.group({
      nameSearch: new FormControl('', [Validators.maxLength(this.maxNameLength)]),
      amount: new FormControl('', [
        Validators.required,
        Validators.min(this.minAmount),
        Validators.max(this.maxAmount),
      ]),
    });

    if (!this.nameSearch) {
      return;
    }

    this.nameSearch.valueChanges
      .pipe(
        tap((val: string) => {
          const newValue = this.inputPipe.transform(val);
          if (val === newValue) {
            return;
          }

          this.nameSearch?.setValue(newValue);
        }),
        untilDestroyed(this)
      )
      .subscribe();
  }
}
