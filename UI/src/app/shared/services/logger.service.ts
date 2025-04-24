import { Injectable } from '@angular/core';
import { NGXLogger } from 'ngx-logger';

@Injectable({
  providedIn: 'root',
})
export class LoggerService {
  constructor(private logger: NGXLogger) {}

  log(message: string, ...params: any[]) {
    this.logger.log(message, ...params);
  }

  info(message: string, ...params: any[]) {
    this.logger.info(message, ...params);
  }

  warning(message: string, ...params: any[]) {
    this.logger.warn(message, ...params);
  }

  error(message: string, ...params: any[]) {
    this.logger.error(message, ...params);
  }

  fatal(message: string, ...params: any[]) {
    this.logger.fatal(message, ...params);
  }
}
