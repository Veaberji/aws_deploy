import { DatePipe } from '@angular/common';
import { TestBed } from '@angular/core/testing';
import { DateService } from './date.service';

describe('DateService', () => {
  const testDate = '2022-02-01T01:01:01.001-01:01';
  const testDateOnly = '2022-02-01';
  let service: DateService;

  beforeEach(() => {
    TestBed.configureTestingModule({ providers: [DatePipe] });
    service = TestBed.inject(DateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('#getDateOnly should return empty string when empty string is passed', () => {
    const result = service.getDateOnly('');

    expect(result).toBe('');
  });

  it('#getDateOnly should return date only string when Date is passed', () => {
    const date = new Date(testDate);

    const result = service.getDateOnly(date);

    expect(result).toBe(testDateOnly);
  });

  it('#getDateOnly should return date only string when string date is passed', () => {
    const result = service.getDateOnly(testDate);

    expect(result).toBe(testDateOnly);
  });

  it('#getDateMonthAgo should return Date one months early than date', () => {
    const dateNow = new Date();

    const result = service.getDateMonthAgo();
    let months = (dateNow.getFullYear() - result.getFullYear()) * 12 + (dateNow.getMonth() - result.getMonth());

    expect(months).toBe(1);
  });

  it('#getOneYearDate should return Date one year later than date', () => {
    const dateNow = new Date();

    const result = service.getOneYearDate();
    let years = result.getFullYear() - dateNow.getFullYear();

    expect(years).toBe(1);
  });
});
