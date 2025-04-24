import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, of, Subject, switchMap } from 'rxjs';
import { AlbumService } from '../../services/album.service';
import { UrlService } from '../../../shared/services/url.service';
import Album from '../../models/album';
import CardItem from '../../../shared/models/card-item';

@Component({
  selector: 'app-albums-container',
  templateUrl: './albums-container.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TopAlbumsContainerComponent implements OnInit {
  private albums$: Observable<Album[]> = new Subject<Album[]>();
  cardItems$: Observable<CardItem[]> = new Subject<CardItem[]>();
  artistName: string = '';
  isDataLoading: boolean = true;

  constructor(private route: ActivatedRoute, private albumService: AlbumService, private urlService: UrlService) {}

  ngOnInit(): void {
    this.initDataStreams();
  }

  private initDataStreams() {
    this.initAlbumsStream();
    this.initCardItemsStream();
  }

  private initAlbumsStream(): void {
    this.artistName = String(this.route.parent?.snapshot.paramMap.get('name'));
    this.albums$ = this.albumService.getTopAlbums(this.artistName);
  }

  private initCardItemsStream() {
    this.cardItems$ = this.albums$?.pipe(
      switchMap((albums) => {
        const items: CardItem[] = albums.map((album) => ({
          title: album.name,
          imageUrl: album.imageUrl,
          navUrl: `/albums/${this.escapeName(album.artistName ?? '')}/${this.escapeName(album.name)}/details`,
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
