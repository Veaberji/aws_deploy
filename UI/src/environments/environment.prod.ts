import { NgxLoggerLevel } from 'ngx-logger';

export const environment = {
  title: '',
  baseApiUrl: '',
  production: true,
  logging: {
    level: NgxLoggerLevel.ERROR,
  },
};
