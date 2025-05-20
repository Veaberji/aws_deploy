import { NgxLoggerLevel } from 'ngx-logger';

export const environment = {
  title: 'MusiciansAPP',
  baseApiUrl: 'http://3.84.53.191:7093/api/',
  production: true,
  logging: {
    level: NgxLoggerLevel.ERROR,
  },
};
