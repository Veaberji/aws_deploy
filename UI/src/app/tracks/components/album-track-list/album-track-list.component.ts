import { DatePipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { Observable, switchMap, zip } from 'rxjs';
import { LanguageService } from 'src/app/shared/services/language.service';
import TableColumn from 'src/app/shared/models/table-column';
import Track from '../../models/track';
import TableRow from 'src/app/shared/models/table-row';

@Component({
  selector: 'app-album-track-list',
  templateUrl: './album-track-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AlbumTrackListComponent implements OnInit {
  columns$: Observable<TableColumn[]> = new Observable();
  tableRows: TableRow[] = [];
  @Input() tracks: Track[] = [];

  constructor(private datePipe: DatePipe, private translate: LanguageService) {}

  ngOnInit(): void {
    this.initTableItems();
    this.initColumnsStream();
  }

  private initTableItems(): void {
    let mappedTracks: TableRow[] = this.tracks.map((track) => ({
      name: track.name,
      duration: track.durationInSeconds ? this.datePipe.transform(track.durationInSeconds * 1000, 'mm:ss') ?? '' : '',
    }));

    this.tableRows = mappedTracks;
  }

  private initColumnsStream(): void {
    this.columns$ = this.translate.currentLanguage$.pipe(switchMap(() => this.columns));
  }

  private get columns(): Observable<TableColumn[]> {
    const name$ = this.translate.getTranslation('track.title');
    const duration$ = this.translate.getTranslation('track.duration');

    return zip([name$, duration$], (name, duration) => {
      return [
        { header: '#', field: '' },
        { header: name, field: 'name' },
        { header: duration, field: 'duration' },
      ];
    });
  }
}
