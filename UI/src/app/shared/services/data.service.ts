import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import {
  map,
  Observable,
  throwError,
  catchError,
  switchMap,
  share,
  retryWhen,
  delay,
  iif,
  concatMap,
  of,
  MonoTypeOperatorFunction,
} from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoggerService } from './logger.service';
import { NotifyService } from './notify.service';
import StatusCode from 'status-code-enum';
import { AppError } from '../common/errors/app-error';
import { NotFoundError } from '../common/errors/not-found-error';
import { ServerError } from '../common/errors/server-error';
import { ServiceUnavailableError } from '../common/errors/service-unavailable-error';

@Injectable({
  providedIn: 'root',
})
export abstract class DataService<T> {
  private readonly apiUrl: string = '';
  private readonly delayInMillisecond: number = 5000;
  private readonly retriesAmount: number = 5;

  constructor(
    @Inject(String) private controllerApi: string,
    private http: HttpClient,
    private logger: LoggerService,
    private notifyService: NotifyService
  ) {
    this.apiUrl = environment.baseApiUrl + controllerApi;
  }

  protected getAll(query?: string): Observable<T[]> {
    const url = query ? `${this.apiUrl}${query}` : this.apiUrl;
    this.logApiCall(url);

    return this.http.get(url).pipe(
      this.retryPipeline,
      map((response) => response as T[]),
      catchError(this.handleError),
      share()
    );
  }

  protected getAllByObservable(query$: Observable<string>): Observable<T[]> {
    return query$.pipe(
      switchMap((query) => {
        const url = query ? `${this.apiUrl}${query}` : this.apiUrl;
        this.logApiCall(url);

        return this.http.get(url).pipe(
          this.retryPipeline,
          map((response) => response as T[]),
          catchError(this.handleError)
        );
      }),
      share()
    );
  }

  protected get(query: string): Observable<T> {
    const url = `${this.apiUrl}${query}`;
    this.logApiCall(url);

    return this.http.get(url).pipe(
      this.retryPipeline,
      map((response) => response as T),
      catchError(this.handleError),
      share()
    );
  }

  protected getByObservable(query$: Observable<string>): Observable<T> {
    return query$.pipe(
      switchMap((query) => {
        const url = `${this.apiUrl}${query}`;
        this.logApiCall(url);

        return this.http.get(url).pipe(
          this.retryPipeline,
          map((response) => response as T),
          catchError(this.handleError)
        );
      }),
      share()
    );
  }

  protected getAmount(query: string): Observable<number> {
    const url = `${this.apiUrl}${query}`;
    this.logApiCall(url);

    return this.http.get(url).pipe(
      this.retryPipeline,
      map((response) => response as number),
      catchError(this.handleError),
      share()
    );
  }

  protected getAmountByObservable(query$: Observable<string>): Observable<number> {
    return query$.pipe(
      switchMap((query) => {
        const url = `${this.apiUrl}${query}`;
        this.logApiCall(url);

        return this.http.get(url).pipe(
          this.retryPipeline,
          map((response) => response as number),
          catchError(this.handleError)
        );
      }),
      share()
    );
  }

  protected download(query: string, body: any): Observable<HttpResponse<Blob>> {
    const url = `${this.apiUrl}${query}`;
    this.logApiCall(url, body);

    return this.http
      .post<Blob>(url, body, {
        observe: 'response',
        responseType: 'blob' as 'json',
      })
      .pipe(this.retryPipeline, catchError(this.handleError));
  }

  private retryPipeline: MonoTypeOperatorFunction<any> = retryWhen((errors: Observable<HttpErrorResponse>) =>
    errors.pipe(
      concatMap((error, count) =>
        iif(
          () => error.status !== StatusCode.ServerErrorServiceUnavailable || count + 1 >= this.retriesAmount,
          throwError(() => error),
          this.notifyError(error, count)
        )
      )
    )
  );

  private notifyError(error: HttpErrorResponse, count: number): Observable<HttpErrorResponse> {
    if (count + 1 < this.retriesAmount) {
      this.notifyService.error(
        `Service Unavailable, will try to reconnect in ${this.delayInMillisecond / 1000} seconds`,
        'Server Error',
        this.delayInMillisecond
      );
    }

    return of(error).pipe(delay(this.delayInMillisecond));
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    if (error.status === StatusCode.ClientErrorNotFound) {
      return throwError(() => new NotFoundError(`Data Not Found: ${error.error}`));
    } else if (error.status === StatusCode.ServerErrorServiceUnavailable) {
      return throwError(() => new ServiceUnavailableError(`ServiceUnavailable: ${error.error}`));
    } else if (error.status >= 500) {
      return throwError(() => new ServerError(`Server returned code ${error.status}: ${error.error}`));
    } else {
      return throwError(() => new AppError('An unexpected error occurred'));
    }
  }

  private logApiCall(url: string, ...params: any[]): void {
    this.logger.info(`Calling API: ${url}`, ...params);
  }
}
