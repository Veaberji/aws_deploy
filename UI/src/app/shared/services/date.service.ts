import { DatePipe } from '@angular/common';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DateService {
  private readonly format = 'yyyy-MM-dd';

  constructor(private datePipe: DatePipe) {}

  getDateOnly(date: string | Date): string {
    return this.datePipe.transform(date, this.format) ?? '';
  }

  getDateMonthAgo(): Date {
    const monthAgo = new Date();
    monthAgo.setMonth(monthAgo.getMonth() - 1);

    return monthAgo;
  }

  getOneYearDate(): Date {
    const date = new Date();
    date.setFullYear(date.getFullYear() + 1);
    return date;
  }
}
