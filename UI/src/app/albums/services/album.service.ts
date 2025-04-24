import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, switchMap } from 'rxjs';
import { LoggerService } from '../../../app/shared/services/logger.service';
import { NotifyService } from '../../../app/shared/services/notify.service';
import { DataService } from '../../shared/services/data.service';
import Album from '../models/album';

@Injectable({
  providedIn: 'root',
})
export class AlbumService extends DataService<Album> {
  constructor(http: HttpClient, logger: LoggerService, notifyService: NotifyService) {
    super('albums/', http, logger, notifyService);
  }

  getTopAlbums(artistName: string): Observable<Album[]> {
    const apiPostfix = 'top-albums';
    return this.getAll(`${artistName}/${apiPostfix}`);
  }

  getAlbumsByDate(query$: Observable<string>): Observable<Album[]> {
    const topAlbumsQuery$ = query$.pipe(
      switchMap((query) => {
        const apiPostfix = 'byDate';
        const url = `${apiPostfix}${query}`;
        return of(url);
      })
    );
    return this.getAllByObservable(topAlbumsQuery$);
  }

  getAlbumsByDateAmount(query$: Observable<string>): Observable<number> {
    const topAlbumsAmountQuery$ = query$.pipe(
      switchMap((query) => {
        const apiPostfix = 'byDate/amount';
        const url = `${apiPostfix}${query}`;
        return of(url);
      })
    );
    return this.getAmountByObservable(topAlbumsAmountQuery$);
  }

  getAlbumDetails(artistName: string, albumTitle: string): Observable<Album> {
    const apiPostfix = 'album-details';
    return this.get(`${artistName}/${apiPostfix}/${albumTitle}`);
  }
}
