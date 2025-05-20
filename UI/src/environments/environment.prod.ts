import { NgxLoggerLevel } from 'ngx-logger';

export const environment = {
  title: 'MusiciansAPP',
  baseApiUrl: 'http://54.86.68.206:7093/api/',
  production: true,
  logging: {
    level: NgxLoggerLevel.ERROR,
  },
};
