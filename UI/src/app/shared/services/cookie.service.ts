import { Injectable } from '@angular/core';
import { CookieOptions, CookieService } from 'ngx-cookie-service';
import { DateService } from './date.service';

@Injectable({
  providedIn: 'root',
})
export class AppCookieService {
  private readonly _defaultPath = '/';
  constructor(private cookieService: CookieService, private dateService: DateService) {}

  get(name: string): string {
    return this.cookieService.get(name);
  }

  set(name: string, value: string, expired?: number | Date): void {
    if (!expired) {
      expired = this.dateService.getOneYearDate();
    }

    const options: CookieOptions = {
      path: this._defaultPath,
      expires: expired,
    };

    this.cookieService.set(name, value, options);
  }

  delete(name: string): void {
    return this.cookieService.delete(name);
  }
}
