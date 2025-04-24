import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { FileService } from './file.service';
import { LoggerService } from './logger.service';
import { NotifyService } from './notify.service';
import ReportRequest from '../models/report-request';

describe('FileService', () => {
  const testRequest: ReportRequest = { name: 'testName', amount: 2 };

  const testFileName: string = 'testFileName';
  const testFileType: string = 'testType';
  const testFileData: Blob = new Blob([''], { type: testFileType });
  const contentDispositionItems = `testAttachment; filename=${testFileName}; filename*=testEncodingFileName`;
  const testResponse: HttpResponse<Blob> = new HttpResponse<Blob>({
    body: testFileData,
    headers: new HttpHeaders({ 'content-disposition': contentDispositionItems }),
  });
  let service: FileService;
  const httpClientSpy = jasmine.createSpyObj('HttpClient', ['post']);
  const loggerServiceSpy = jasmine.createSpyObj('LoggerService', ['info']);
  const notifyServiceSpy = jasmine.createSpyObj('NotifyService', ['error']);

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        { provide: HttpClient, useValue: httpClientSpy },
        { provide: LoggerService, useValue: loggerServiceSpy },
        { provide: NotifyService, useValue: notifyServiceSpy },
      ],
    });
    service = TestBed.inject(FileService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('#getArtistsReport should return correct data', () => {
    httpClientSpy.post.and.returnValue(of(testResponse));

    const result = service.getArtistsReport(testRequest);

    result.subscribe((file) => {
      expect(file.name).toBe('testFileName');
      expect(file.data.type.toLowerCase()).toBe(testFileType.toLowerCase());
    });
  });
});
