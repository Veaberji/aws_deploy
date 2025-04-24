import { Component, Input } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Title } from '@angular/platform-browser';
import { of } from 'rxjs';

import { FileService } from 'src/app/shared/services/file.service';
import { LanguageService } from 'src/app/shared/services/language.service';
import { ArtistsReportComponent } from './artists-report.component';
import { Language } from 'src/app/shared/models/language';

describe('ArtistsReportComponent', () => {
  let component: ArtistsReportComponent;
  let fixture: ComponentFixture<ArtistsReportComponent>;
  const fileServiceSpy = jasmine.createSpyObj('FileService', ['getArtistsReport']);
  const titleServiceSpy = jasmine.createSpyObj('Title', ['setTitle']);
  const languageServiceSpy = jasmine.createSpyObj('LanguageService', ['use', 'getTranslation']);
  let testTitle: string;

  beforeEach(async () => {
    @Component({ selector: 'app-report-form', template: '' })
    class AppReportFormComponentStub {
      @Input('isDataLoading') isDataLoading: boolean = false;
      @Input('minAmount') minAmount: number = 0;
      @Input('maxAmount') maxAmount: number = 0;
    }
    @Component({ selector: 'app-loading', template: '' })
    class AppLoadingComponentStub {}
    await TestBed.configureTestingModule({
      declarations: [ArtistsReportComponent, AppReportFormComponentStub, AppLoadingComponentStub],
      providers: [
        { provide: Title, useValue: titleServiceSpy },
        { provide: FileService, useValue: fileServiceSpy },
        { provide: LanguageService, useValue: languageServiceSpy },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    languageServiceSpy.currentLanguage$ = of(Language.English);
    testTitle = 'testTitle';
    languageServiceSpy.getTranslation.and.returnValue(of(testTitle));

    fixture = TestBed.createComponent(ArtistsReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });

  it('should set title when the current language change', () => {
    languageServiceSpy.currentLanguage$.subscribe(() => {
      expect(titleServiceSpy.setTitle).toHaveBeenCalledWith(jasmine.stringMatching(testTitle));
    });
  });
});
