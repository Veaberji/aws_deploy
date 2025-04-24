import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, map, Observable, switchMap, zip } from 'rxjs';
import { TrackService } from '../../services/track.service';
import { LanguageService } from 'src/app/shared/services/language.service';
import Track from '../../models/track';
import TableColumn from 'src/app/shared/models/table-column';
import TableRow from 'src/app/shared/models/table-row';

@Component({
  selector: 'app-track-list',
  templateUrl: './track-list.component.html',
  styleUrls: ['./track-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TopTrackListComponent implements OnInit {
  private artistName: string = '';
  private readonly size: number = 10;
  private readonly page: BehaviorSubject<number> = new BehaviorSubject<number>(1);
  private readonly page$: Observable<number> = this.page.asObservable();
  tableRows$: Observable<TableRow[]> = new Observable<TableRow[]>();
  totalItems$: Observable<number> = new Observable<number>();
  columns$: Observable<TableColumn[]> = new Observable();

  constructor(private route: ActivatedRoute, private trackService: TrackService, private translate: LanguageService) {}

  ngOnInit(): void {
    this.initArtistName();
    this.initDataStreams();
  }

  onLoadMore() {
    const value = this.page.value;
    this.page.next(value + 1);
  }

  private initArtistName() {
    this.artistName = String(this.route.parent?.snapshot.paramMap.get('name'));
  }

  private initDataStreams() {
    this.initTableRowsStream();
    this.initColumnsStream();
    this.initTotalItemsStream();
  }

  private initTableRowsStream() {
    const pagingQuery$ = this.page$.pipe(map((page) => `?pageSize=${this.size}&page=${page}`));
    const newTracks$ = this.trackService.getTopTracks(this.artistName, pagingQuery$);

    let buffered: Track[] = [];
    const tracks$: Observable<Track[]> = newTracks$.pipe(
      map((tracks) => {
        buffered = [...buffered, ...tracks];
        return buffered;
      })
    );

    this.tableRows$ = tracks$.pipe(
      map((tracks) =>
        tracks.map((track) => ({
          name: track.name,
          timesPlayed: track.playCount.toString(),
        }))
      )
    );
  }

  private initTotalItemsStream() {
    this.totalItems$ = this.trackService.getTopTracksAmount(this.artistName);
  }

  private initColumnsStream(): void {
    this.columns$ = this.translate.currentLanguage$.pipe(switchMap(() => this.columns));
  }

  private get columns(): Observable<TableColumn[]> {
    const title$ = this.translate.getTranslation('track.title');
    const timesPlayed$ = this.translate.getTranslation('track.timesPlayed');

    return zip([title$, timesPlayed$], (title, timesPlayed) => {
      return [
        { header: '#', field: '' },
        { header: title, field: 'name' },
        { header: timesPlayed, field: 'timesPlayed' },
      ];
    });
  }
}
