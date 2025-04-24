import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { AppCookieService } from './cookie.service';
import { LoggerService } from './logger.service';
import { Language } from '../models/language';
import LanguageDisplay from '../models/language-display';

@Injectable({
  providedIn: 'root',
})
export class LanguageService {
  private readonly languageCookie = 'Language';
  private _languages!: LanguageDisplay[];
  private currentLanguage!: BehaviorSubject<Language>;
  currentLanguage$!: Observable<Language>;

  constructor(
    private cookieService: AppCookieService,
    private logger: LoggerService,
    private translate: TranslateService
  ) {
    this.init();
  }

  get languages(): LanguageDisplay[] {
    return this._languages;
  }

  use(language: Language): void {
    this.translate.use(language);
    this.cookieService.set(this.languageCookie, language);
    this.logger.info('CurrentLanguage changed to', this.getLanguageDisplayName(language));
    this.currentLanguage.next(language);
  }

  getTranslation(key: string): Observable<string> {
    return this.translate.stream(key);
  }

  private init(): void {
    let language = this.cookieService.get(this.languageCookie) as Language;
    language = language ? language : Language.English;

    this.translate.addLangs(this.languagesKeys);
    this.translate.setDefaultLang(language);

    this.currentLanguage = new BehaviorSubject<Language>(language);
    this.currentLanguage$ = this.currentLanguage.asObservable();

    this._languages = this.languagesKeys.map((key) => ({
      key: key as Language,
      display: this.getLanguageDisplayName(key),
    }));
  }

  private get languagesKeys(): string[] {
    return Object.values(Language);
  }

  private getLanguageDisplayName(key: string): string {
    return new Intl.DisplayNames([key], { type: 'language' }).of(key) ?? key;
  }
}
