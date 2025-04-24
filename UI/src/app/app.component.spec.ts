import { Component } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { environment } from '../environments/environment';
import { LoggerService } from './shared/services/logger.service';
import { SettingsService } from './shared/services/settings.service';
import { ThemeService } from './shared/services/theme.service';
import { LanguageService } from './shared/services/language.service';
import { AppComponent } from './app.component';

describe('AppComponent', () => {
  beforeEach(async () => {
    @Component({ selector: 'app-nav-bar', template: '' })
    class NavBarComponentStub {}

    @Component({ selector: 'app-footer', template: '' })
    class FooterComponentStub {}

    const settingsServiceSpy = jasmine.createSpyObj('SettingsService', ['getThemeSettings']);
    const themeServiceSpy = jasmine.createSpyObj('ThemeService', [
      'getTheme',
      'changeBaseTheme',
      'useBaseTheme',
      'useTimeBasedTheme',
      'timeBasedThemeUsed',
      'setLightTimeBasedTheme',
      'setDarkTimeBasedTheme',
    ]);
    const loggerServiceSpy = jasmine.createSpyObj('LoggerService', ['info']);
    const languageServiceSpy = jasmine.createSpyObj('LanguageService', ['addTranslation']);

    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        { provide: SettingsService, useValue: settingsServiceSpy },
        { provide: ThemeService, useValue: themeServiceSpy },
        { provide: LoggerService, useValue: loggerServiceSpy },
        { provide: LanguageService, useValue: languageServiceSpy },
      ],
      declarations: [AppComponent, NavBarComponentStub, FooterComponentStub],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);

    const app = fixture.componentInstance;

    expect(app).toBeTruthy();
  });

  it(`should have title same as in as environment.title`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const expectedTitle = environment.title;

    const app = fixture.componentInstance;

    expect(app.title).toEqual(expectedTitle);
  });
});
