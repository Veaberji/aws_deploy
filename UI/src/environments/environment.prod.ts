import { NgxLoggerLevel } from 'ngx-logger';

export const environment = {
  title: 'MusiciansAPP',
  baseApiUrl: 'http://13.219.91.135:7093/api/',
  production: true,
  logging: {
    level: NgxLoggerLevel.ERROR,
  },
};
