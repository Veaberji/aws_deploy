import { NgxLoggerLevel } from 'ngx-logger';

export const environment = {
  title: 'MusiciansAPP',
  baseApiUrl: 'http://localhost:7093/api/',
  production: true,
  logging: {
    level: NgxLoggerLevel.ERROR,
  },
};
