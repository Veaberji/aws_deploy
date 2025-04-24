import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { map, Observable, switchMap, of, zip, BehaviorSubject, tap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { environment } from '../../../../environments/environment';
import { ArtistService } from '../../services/artist.service';
import { LanguageService } from 'src/app/shared/services/language.service';
import { UrlService } from '../../../shared/services/url.service';
import CardItem from '../../../shared/models/card-item';
import Paging from '../../../shared/models/paging';
import Artist from '../../models/artist';

@UntilDestroy()
@Component({
  selector: 'app-artists-container',
  templateUrl: './artists-container.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ArtistsContainerComponent implements OnInit {
  readonly pageSizes: [12, 24, 48] = [12, 24, 48];
  private currentPageSize: number = this.pageSizes[0];
  private topArtists$: Observable<Artist[]> = new Observable<Artist[]>();
  private totalItems: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  totalItems$: Observable<number> = this.totalItems.asObservable();
  paging$: Observable<Paging> = new Observable<Paging>();
  cardItems$: Observable<CardItem[]> = new Observable<CardItem[]>();
  isDataLoading: boolean = true;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private artistService: ArtistService,
    private titleService: Title,
    private urlService: UrlService,
    private translate: LanguageService
  ) {}

  ngOnInit(): void {
    this.initDataStreams();
    this.initTitle();
  }

  onPageChange(page: number): void {
    const url = `artists/page/${page}/pageSize/${this.currentPageSize}`;
    this.router.navigate([url]);
  }

  onPageSizeChange(size: number): void {
    this.currentPageSize = size;
    const firstPage = 1;
    const url = `artists/page/${firstPage}/pageSize/${size}`;
    this.router.navigate([url]);
  }

  private initDataStreams() {
    this.initTotalArtistsStream();
    this.initTopArtistsStream();
    this.initCardItemsStream();
  }

  private initTopArtistsStream(): void {
    this.initPagingStream();

    const pagingQuery$ = this.paging$.pipe(
      map((paging) => {
        this.isDataLoading = true;
        return `?pageSize=${paging.pageSize}&page=${paging.page}`;
      })
    );
    this.topArtists$ = this.artistService.getTopArtists(pagingQuery$);
  }

  private initPagingStream() {
    const params = this.route.params;
    const currentPage$ = params.pipe(map((param) => +param['page']));
    const currentPageSize$ = params.pipe(map((param) => +param['pageSize']));

    this.paging$ = zip([currentPage$, currentPageSize$], (page, pageSize) => {
      page = !page ? 1 : page;
      pageSize = !pageSize ? this.pageSizes[0] : pageSize;
      return { page: page, pageSize: pageSize };
    });
  }

  private initCardItemsStream() {
    this.cardItems$ = this.topArtists$?.pipe(
      switchMap((artists) => {
        const items: CardItem[] = artists.map((artist) => ({
          title: artist.name,
          imageUrl: artist.imageUrl,
          navUrl: `/artists/details/${this.escapeName(artist.name)}`,
        }));
        this.isDataLoading = false;

        return of(items);
      })
    );
  }

  private initTotalArtistsStream() {
    this.totalItems.next(this.artistService.getTotalTopArtists());
  }

  private escapeName(name: string): string {
    return this.urlService.escape(name);
  }

  private initTitle() {
    this.translate
      .getTranslation('topArtists.pageTitle')
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
