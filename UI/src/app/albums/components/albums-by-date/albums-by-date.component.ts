import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { distinctUntilChanged, map, Observable, of, switchMap, tap, zip, combineLatest } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { environment } from '../../../../environments/environment';
import { LanguageService } from '../../../shared/services/language.service';
import { AlbumService } from '../../services/album.service';
import { UrlService } from './../../../shared/services/url.service';
import { DateService } from 'src/app/shared/services/date.service';
import CardItem from '../../../shared/models/card-item';
import Dates from '../../../shared/models/dates';
import Album from '../../models/album';
import Paging from '../../../shared/models/paging';

@UntilDestroy()
@Component({
  selector: 'app-albums-by-date',
  templateUrl: './albums-by-date.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AlbumsByDateComponent implements OnInit {
  readonly pageSizes: [12, 24, 48] = [12, 24, 48];
  private currentPageSize: number = this.pageSizes[0];
  private startDate: Date = new Date();
  private endDate: Date = new Date();
  private datesQuery$: Observable<string> = new Observable<string>();
  private albums$: Observable<Album[]> = new Observable<Album[]>();
  isDataLoading: boolean = true;
  totalItems$: Observable<number> = new Observable<number>();
  paging$: Observable<Paging> = new Observable<Paging>();
  dates$: Observable<Dates> = new Observable<Dates>();
  cardItems$: Observable<CardItem[]> = new Observable<CardItem[]>();

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private albumService: AlbumService,
    private titleService: Title,
    private urlService: UrlService,
    private dateService: DateService,
    private translate: LanguageService
  ) {}

  ngOnInit(): void {
    this.initDataStreams();
    this.initTitle();
  }

  onPageChange(page: number): void {
    this.navigate(this.startDate, this.endDate, page, this.currentPageSize);
  }

  onPageSizeChange(size: number): void {
    this.currentPageSize = size;
    const firstPage = 1;
    this.navigate(this.startDate, this.endDate, firstPage, size);
  }

  onDatesSelected(dates: Dates): void {
    const firstPage = 1;
    this.navigate(dates.startDate, dates.endDate, firstPage, this.currentPageSize);
  }

  private escapeName(name: string): string {
    return this.urlService.escape(name);
  }

  private initDataStreams() {
    this.initDatesQueryStream();
    this.initAlbumsStream();
    this.initTotalItemsStream();
    this.initCardItemsStream();
  }

  private initDatesQueryStream(): void {
    this.initDatesStream();
    this.datesQuery$ = this.dates$.pipe(
      map((dates) => {
        return `startDate=${this.dateService.getDateOnly(dates.startDate)}&endDate=${this.dateService.getDateOnly(
          dates.endDate
        )}`;
      })
    );
  }

  private initAlbumsStream(): void {
    this.initPagingStream();

    const pagingQuery$ = this.paging$.pipe(map(({ page, pageSize }) => `pageSize=${pageSize}&page=${page}`));
    const query$ = zip([this.datesQuery$, pagingQuery$], (dates, paging) => {
      this.isDataLoading = true;
      return `?${dates}&${paging}`;
    });

    this.albums$ = this.albumService.getAlbumsByDate(query$);
  }

  private initPagingStream(): void {
    const params = this.route.queryParams;
    const currentPage$ = params.pipe(map((param) => +param['page']));
    const currentPageSize$ = params.pipe(map((param) => +param['pageSize']));

    this.paging$ = zip([currentPage$, currentPageSize$], (page, pageSize) => {
      page = !page ? 1 : page;
      pageSize = !pageSize ? this.pageSizes[0] : pageSize;
      return { page, pageSize };
    });
  }

  private initTotalItemsStream() {
    const totalItemsQuery$ = this.datesQuery$.pipe(
      distinctUntilChanged(),
      switchMap((dates) => {
        return of(`?${dates}`);
      })
    );

    this.totalItems$ = this.albumService.getAlbumsByDateAmount(totalItemsQuery$);
  }

  private initDatesStream(): void {
    const params = this.route.queryParams;
    const startDate$: Observable<Date> = params.pipe(
      map((param) => {
        const date = param['start'];
        return date ? new Date(date) : this.dateService.getDateMonthAgo();
      })
    );
    const endDate$: Observable<Date> = params.pipe(
      map((param) => {
        const date = param['end'];
        return date ? new Date(date) : new Date();
      })
    );

    this.dates$ = zip([startDate$, endDate$], (startDate, endDate) => {
      this.startDate = startDate;
      this.endDate = endDate;
      return { startDate, endDate };
    });
  }

  private initCardItemsStream() {
    this.cardItems$ = combineLatest([this.translate.getTranslation('album.created'), this.albums$]).pipe(
      switchMap(([created, albums]) => {
        const items: CardItem[] = albums.map((album) => ({
          title: album.name,
          imageUrl: album.imageUrl,
          navUrl: `/albums/${this.escapeName(album.artistName ?? '')}/${this.escapeName(album.name)}/details`,
          newWindow: true,
          content: `${created}: ${new Date(album.dateCreated).toLocaleDateString()}`,
        }));
        this.isDataLoading = false;

        return of(items);
      })
    );
  }

  private navigate(startDate: Date, endDate: Date, page: number, pageSize: number): void {
    this.router.navigate([], {
      queryParams: {
        start: this.dateService.getDateOnly(startDate),
        end: this.dateService.getDateOnly(endDate),
        page: page,
        pageSize: pageSize,
      },
    });
  }

  private initTitle() {
    this.translate
      .getTranslation('albumsByDate.pageTitle')
      .pipe(
        tap((translation) => this.setTitle(translation)),
        untilDestroyed(this)
      )
      .subscribe();
  }

  private setTitle(translation: string) {
    this.titleService.setTitle(`${environment.title} - ${translation}`);
  }
}
