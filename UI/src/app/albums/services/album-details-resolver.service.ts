import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { catchError, EMPTY, Observable } from 'rxjs';
import { AppErrorHandler } from 'src/app/shared/common/errors/app-error-handler';
import { AlbumService } from './album.service';
import Album from '../models/album';

@Injectable({
  providedIn: 'root',
})
export class AlbumDetailsResolver implements Resolve<Album> {
  constructor(private service: AlbumService, private errorHandler: AppErrorHandler) {}

  resolve(route: ActivatedRouteSnapshot, _: RouterStateSnapshot): Observable<Album> {
    let params = route.paramMap;
    const albumTitle = String(params.get('albumTitle'));
    const artistName = String(params.get('artistName'));

    return this.service.getAlbumDetails(artistName, albumTitle).pipe(
      catchError((error) => {
        this.errorHandler.handleError(error as Error);
        return EMPTY;
      })
    );
  }
}
