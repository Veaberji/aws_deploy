import { TestBed } from '@angular/core/testing';
import { TranslateService } from '@ngx-translate/core';
import { AppCookieService } from './cookie.service';
import { LanguageService } from './language.service';
import { LoggerService } from './logger.service';
import { Language } from '../models/language';
import LanguageDisplay from '../models/language-display';
import { of } from 'rxjs';

describe('LangService', () => {
  let service: LanguageService;
  const cookieServiceSpy = jasmine.createSpyObj('AppCookieService', ['get', 'set']);
  const loggerServiceSpy = jasmine.createSpyObj('LoggerService', ['info']);
  const translateServiceSpy = jasmine.createSpyObj('LanguageService', ['addLangs', 'setDefaultLang', 'stream', 'use']);
  let language: Language;
  const chineseDisplay = '中文';

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        { provide: AppCookieService, useValue: cookieServiceSpy },
        { provide: LoggerService, useValue: loggerServiceSpy },
        { provide: TranslateService, useValue: translateServiceSpy },
      ],
    });
    service = TestBed.inject(LanguageService);
    language = Language.English;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('#languages should return correct languages array when language cookie has been set', () => {
    const expected: LanguageDisplay[] = [
      { key: Language.English, display: 'English' },
      { key: Language.Chinese, display: chineseDisplay },
    ];

    const result = service.languages;

    expect(result).toEqual(expected);
  });

  it('#use should call TranslateService`s use with correct data when called', () => {
    service.use(language);

    expect(translateServiceSpy.use).toHaveBeenCalledWith(language);
  });

  it('#use should call AppCookieService`s set with correct data when called', () => {
    service.use(language);

    expect(cookieServiceSpy.set).toHaveBeenCalledWith(jasmine.any(String), language);
  });

  it('#use should call LoggerService`s info with correct data when called', () => {
    language = Language.Chinese;

    service.use(language);

    expect(loggerServiceSpy.info).toHaveBeenCalledWith(jasmine.any(String), chineseDisplay);
  });

  it('#use should call currentLanguage`s next with correct data when called', () => {
    language = Language.Chinese;

    service.use(language);

    service.currentLanguage$.subscribe((lang) => {
      expect(lang.toLowerCase()).toBe('zh');
    });
  });

  it('#getTranslation should return correct data when called', () => {
    const testValue = 'testValue';
    translateServiceSpy.stream.and.returnValue(of(testValue));

    const result$ = service.getTranslation('test');

    result$.subscribe((translation) => {
      expect(translation).toBe(testValue);
    });
  });
});
