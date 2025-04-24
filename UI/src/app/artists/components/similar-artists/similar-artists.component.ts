import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map, Observable, of, Subject, switchMap } from 'rxjs';
import { ArtistService } from '../../services/artist.service';
import { UrlService } from '../../../shared/services/url.service';
import Artist from '../../models/artist';
import CardItem from '../../../shared/models/card-item';
import GridSizes from '../../../shared/models/grid-sizes';

@Component({
  selector: 'app-similar-artists',
  templateUrl: './similar-artists.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SimilarArtistsComponent implements OnInit {
  readonly sizes: GridSizes = {
    small: 1,
    middle: 2,
    large: 3,
    extraLarge: 5,
  };
  private artists$: Observable<Artist[]> = new Subject<Artist[]>();
  cardItems$: Observable<CardItem[]> = new Subject<CardItem[]>();
  isDataLoading: boolean = true;

  constructor(private route: ActivatedRoute, private artistService: ArtistService, private urlService: UrlService) {}

  ngOnInit(): void {
    this.initDataStreams();
  }

  private initDataStreams() {
    this.initSimilarArtists();
    this.initCardItems();
  }

  private initSimilarArtists(): void {
    const artistName$ = this.route.parent?.params.pipe(
      map((param) => {
        this.isDataLoading = true;
        return param['name'];
      })
    );
    if (artistName$) {
      this.artists$ = this.artistService.getSimilarArtists(artistName$);
    }
  }

  private initCardItems() {
    this.cardItems$ = this.artists$?.pipe(
      switchMap((artists) => {
        const items: CardItem[] = artists.map((artist) => ({
          title: artist.name,
          imageUrl: artist.imageUrl,
          navUrl: `/artists/details/${this.escapeName(artist.name)}`,
          newWindow: true,
        }));
        this.isDataLoading = false;

        return of(items);
      })
    );
  }

  private escapeName(name: string): string {
    return this.urlService.escape(name);
  }
}
