import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { NotifyService } from './notify.service';
import { DataService } from './data.service';
import { LoggerService } from './logger.service';
import { AppError } from '../common/errors/app-error';
import ReportRequest from '../models/report-request';
import File from '../models/file';

@Injectable({
  providedIn: 'root',
})
export class FileService extends DataService<any> {
  constructor(http: HttpClient, logger: LoggerService, notifyService: NotifyService) {
    super('file/', http, logger, notifyService);
  }

  getArtistsReport(request: ReportRequest): Observable<File> {
    const apiPostfix = 'artists-report';
    return this.download(`${apiPostfix}`, request).pipe(
      map((resp) => {
        const contentDisposition = resp.headers.get('content-disposition');
        const contentDelimiter = '; ';
        const fileNameField = 'filename=';
        if (!contentDisposition) {
          throw new AppError(`Content-disposition Not Found in Headers: ${resp.headers}`);
        }
        const fileName =
          contentDisposition
            .split(contentDelimiter)
            .find((str) => str.includes(fileNameField))
            ?.replace(fileNameField, '') ?? '';
        const file = resp.body as Blob;

        return { name: fileName, data: file };
      })
    );
  }
}
