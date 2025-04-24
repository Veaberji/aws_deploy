import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of, switchMap } from 'rxjs';
import { LoggerService } from '../../../app/shared/services/logger.service';
import { NotifyService } from '../../../app/shared/services/notify.service';
import { DataService } from '../../shared/services/data.service';
import Artist from '../models/artist';

@Injectable({
  providedIn: 'root',
})
export class ArtistService extends DataService<Artist> {
  constructor(http: HttpClient, logger: LoggerService, notifyService: NotifyService) {
    super('artists/', http, logger, notifyService);
  }

  getTopArtists(query$: Observable<string>): Observable<Artist[]> {
    const apiPostfix = 'top';
    const topArtistsQuery$ = query$.pipe(
      switchMap((query) => {
        const url = `${apiPostfix}${query}`;
        return of(url);
      })
    );
    return this.getAllByObservable(topArtistsQuery$);
  }

  getTotalTopArtists(): number {
    const totalTopArtists = 4295628;
    return totalTopArtists;
  }

  getArtistDetails(name: string): Observable<Artist> {
    const apiPostfix = 'details';
    return this.get(`${name}/${apiPostfix}`);
  }

  getSimilarArtists(artistName$: Observable<string>): Observable<Artist[]> {
    const apiPostfix = 'similar';
    const query$: Observable<string> = artistName$.pipe(map((name) => `${name}/${apiPostfix}`));
    return this.getAllByObservable(query$);
  }
}
