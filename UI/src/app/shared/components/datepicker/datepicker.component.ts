import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable, tap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { DateService } from '../../services/date.service';
import { LoggerService } from '../../services/logger.service';
import Dates from '../../models/dates';

@UntilDestroy()
@Component({
  selector: 'app-datepicker',
  templateUrl: './datepicker.component.html',
  styleUrls: ['./datepicker.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DatepickerComponent implements OnInit {
  dateRangeForm: FormGroup = new FormGroup({ start: new FormControl(), end: new FormControl() });
  @Output() onSubmit = new EventEmitter<Dates>();
  @Input() dates$: Observable<Dates> = new Observable<Dates>();

  constructor(private builder: FormBuilder, private dateService: DateService, private logger: LoggerService) {}

  ngOnInit(): void {
    this.initForm();
  }

  get startDate(): AbstractControl | null {
    return this.dateRangeForm.get('start');
  }

  get endDate(): AbstractControl | null {
    return this.dateRangeForm.get('end');
  }

  onFormSubmit(): void {
    const dates: Dates = {
      startDate: this.getSelectedDate(this.startDate),
      endDate: this.getSelectedDate(this.endDate),
    };

    this.onSubmit.emit(dates);
  }

  onInputChanged(details: string, text: string): void {
    this.logger.info(`${details}: ${text}`);
  }

  private initForm() {
    this.dates$
      .pipe(
        tap((dates) => {
          this.dateRangeForm = this.builder.group({
            start: [dates.startDate.toISOString(), Validators.required],
            end: [dates.endDate.toISOString(), Validators.required],
          });
        }),
        untilDestroyed(this)
      )
      .subscribe();
  }

  private getSelectedDate(dateControl: AbstractControl | null): Date {
    const dateOnly: string = this.dateService.getDateOnly(dateControl?.value);
    return new Date(dateOnly);
  }
}
