import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DataService } from './data.service';
import { LoggerService } from './logger.service';
import { NotifyService } from './notify.service';
import ThemeSettings from '../models/theme-settings';

@Injectable({
  providedIn: 'root',
})
export class SettingsService extends DataService<ThemeSettings> {
  constructor(http: HttpClient, logger: LoggerService, notifyService: NotifyService) {
    super('settings/', http, logger, notifyService);
  }
  getThemeSettings(): Observable<ThemeSettings> {
    const apiPostfix = 'isDark';
    return this.get(`${apiPostfix}/${new Date().toLocaleTimeString()}`);
  }
}
