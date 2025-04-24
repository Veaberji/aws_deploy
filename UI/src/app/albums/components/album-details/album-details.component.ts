import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { switchMap, tap, Observable, of } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { environment } from 'src/environments/environment';
import { LanguageService } from 'src/app/shared/services/language.service';
import NavLink from '../../../shared/models/nav-link';
import Album from '../../models/album';

@UntilDestroy()
@Component({
  selector: 'app-album-details',
  templateUrl: './album-details.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AlbumDetailsComponent implements OnInit {
  album: Album = {
    name: '',
    imageUrl: '',
    playCount: 0,
    artistName: '',
    dateCreated: '',
    description: '',
    tracks: [],
  };
  navLink$!: Observable<NavLink>;

  constructor(private route: ActivatedRoute, private titleService: Title, private translate: LanguageService) {}

  ngOnInit(): void {
    this.initAlbum();
    this.initNavLink();
    this.initTitle();
  }

  private initAlbum(): void {
    this.album = this.route.snapshot.data['album'];
  }

  private initNavLink(): void {
    this.navLink$ = this.translate.getTranslation('albumDetails.by').pipe(
      switchMap((by) => {
        return of({
          link: ['/artists', 'details', this.album.artistName],
          title: `${by} ${this.album.artistName ?? 'Some Artist'}`,
        });
      })
    );
  }

  private initTitle(): void {
    this.translate
      .getTranslation('albumDetails.pageTitle')
      .pipe(
        tap((translation) => this.setTitle(translation)),
        untilDestroyed(this)
      )
      .subscribe();
  }

  private setTitle(translation: string): void {
    this.titleService.setTitle(`${environment.title} - ${this.album?.name} - ${translation}`);
  }
}
