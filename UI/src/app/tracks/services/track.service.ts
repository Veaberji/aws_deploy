import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, switchMap } from 'rxjs';
import { LoggerService } from '../../../app/shared/services/logger.service';
import { NotifyService } from '../../../app/shared/services/notify.service';
import { DataService } from '../../shared/services/data.service';
import Track from '../models/track';

@Injectable({
  providedIn: 'root',
})
export class TrackService extends DataService<Track> {
  constructor(http: HttpClient, logger: LoggerService, notifyService: NotifyService) {
    super('tracks/', http, logger, notifyService);
  }

  getTopTracks(artistName: string, query$: Observable<string>): Observable<Track[]> {
    const topTracksQuery$ = query$.pipe(
      switchMap((query) => {
        const apiPostfix = 'top-tracks';
        const url = `${artistName}/${apiPostfix}${query}`;
        return of(url);
      })
    );
    return this.getAllByObservable(topTracksQuery$);
  }

  getTopTracksAmount(artistName: string): Observable<number> {
    const apiPostfix = 'top-tracks/amount';

    return this.getAmount(`${artistName}/${apiPostfix}`);
  }
}
