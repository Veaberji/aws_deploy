import { ErrorHandler, Injectable, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { LoggerService } from '../../services/logger.service';
import { NotifyService } from '../../services/notify.service';
import { ServiceUnavailableError } from './service-unavailable-error';
import { NotFoundError } from './not-found-error';
import { ServerError } from './server-error';

@Injectable({
  providedIn: 'root',
})
export class AppErrorHandler extends ErrorHandler {
  constructor(
    private router: Router,
    private zone: NgZone,
    private logger: LoggerService,
    private notifyService: NotifyService
  ) {
    super();
  }

  override handleError(error: Error): void {
    this.logger.error(error.message);

    if (error instanceof NotFoundError) {
      this.zone.run(() => this.router.navigate(['/not-found']));
    } else if (error instanceof ServiceUnavailableError) {
      const message = error.message.includes('Blob') ? error.message.split(':')[0] : error.message;
      this.notifyService.error(`${message} - Try again later`, 'Remote Service Error');
    } else if (error instanceof ServerError) {
      this.notifyService.error(error.message, 'Server Error', 5000);
    }
  }
}
