import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { AppCookieService } from './cookie.service';
import { SettingsService } from './settings.service';
import { Theme } from '../models/theme';
import { ThemeType } from '../models/theme-type';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private readonly usedTheme = 'UsedTheme';
  private readonly defaultTheme: Theme = Theme.Light;
  private baseTheme!: Theme;
  private timeBasedTheme!: Theme;
  private _timeBasedThemeUsed!: boolean;
  private theme!: BehaviorSubject<Theme>;
  private timeBasedThemeUsed!: BehaviorSubject<boolean>;
  theme$!: Observable<Theme>;
  timeBasedThemeUsed$!: Observable<boolean>;

  constructor(private cookieService: AppCookieService, private settingsService: SettingsService) {
    this.initBaseTheme();
    this.initTimeBasedTheme();
    this.initDataStreams();
  }

  changeBaseTheme(): void {
    const newTheme = this.baseTheme === Theme.Light ? Theme.Dark : Theme.Light;
    this.cookieService.set(ThemeType.Base, newTheme);
    this.baseTheme = newTheme;
    this.use(ThemeType.Base);
  }

  switchTimeBasedTheme(turnedOn: boolean): void {
    if (turnedOn) {
      this.use(ThemeType.TimeBased);
    } else {
      this.use(ThemeType.Base);
    }
  }

  private initBaseTheme(): void {
    const baseTheme = this.cookieService.get(ThemeType.Base) as Theme;
    if (!baseTheme) {
      this.cookieService.set(ThemeType.Base, this.defaultTheme);
    }
    this.baseTheme = baseTheme ? baseTheme : this.defaultTheme;
  }

  private initTimeBasedTheme(): void {
    if (!environment.production) {
      return;
    }

    this._timeBasedThemeUsed = (this.cookieService.get(this.usedTheme) as ThemeType) === ThemeType.TimeBased;

    this.settingsService.getThemeSettings().subscribe((settings) => {
      if (settings.isDarkTheme) {
        this.setTimeBasedTheme(Theme.Dark);
        this.timeBasedTheme = Theme.Dark;
      } else {
        this.setTimeBasedTheme(Theme.Light);
        this.timeBasedTheme = Theme.Light;
      }
      if (this._timeBasedThemeUsed) {
        this.theme.next(this.timeBasedTheme);
      }
    });
  }

  private initDataStreams(): void {
    const theme: Theme = this._timeBasedThemeUsed ? this.timeBasedTheme : this.baseTheme;

    this.theme = new BehaviorSubject<Theme>(theme);
    this.theme$ = this.theme.asObservable();

    this.timeBasedThemeUsed = new BehaviorSubject<boolean>(this._timeBasedThemeUsed);
    this.timeBasedThemeUsed$ = this.timeBasedThemeUsed.asObservable();
  }

  private setTimeBasedTheme(theme: Theme): void {
    this.cookieService.set(ThemeType.TimeBased, theme);
  }

  private use(type: ThemeType): void {
    this.cookieService.set(this.usedTheme, type);
    if (type === ThemeType.TimeBased) {
      this.theme.next(this.timeBasedTheme);
      this._timeBasedThemeUsed = true;

      // this line is needed to make the theme changes from time-based to base visible to the user
      this.baseTheme = this.timeBasedTheme;
    } else {
      this.theme.next(this.baseTheme);
      this._timeBasedThemeUsed = false;
    }
    this.timeBasedThemeUsed.next(this._timeBasedThemeUsed);
  }
}
