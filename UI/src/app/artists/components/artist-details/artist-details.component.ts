import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, Observable, tap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { environment } from '../../../../environments/environment';
import { LanguageService } from 'src/app/shared/services/language.service';
import { ArtistService } from '../../services/artist.service';
import { SupplementRoute } from '../../supplementRoutes';
import Artist from '../../models/artist';

@UntilDestroy()
@Component({
  selector: 'app-artist-details',
  templateUrl: './artist-details.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ArtistDetailsComponent implements OnInit {
  readonly supplementRoute = SupplementRoute;
  artist$: Observable<Artist> = new Observable<Artist>();

  constructor(
    private route: ActivatedRoute,
    private artistService: ArtistService,
    private titleService: Title,
    private translate: LanguageService
  ) {}

  ngOnInit(): void {
    this.initArtist();
    this.initTitle();
  }

  private initArtist(): void {
    const name = String(this.route.snapshot.paramMap.get('name'));
    this.artist$ = this.artistService.getArtistDetails(name);
  }

  private initTitle(): void {
    combineLatest([this.translate.getTranslation('artistDetails.pageTitle'), this.artist$])
      .pipe(
        tap(([translation, artist]) => this.setTitle(artist.name, translation)),
        untilDestroyed(this)
      )
      .subscribe();
  }

  private setTitle(artistName: string, translation: string) {
    this.titleService.setTitle(`${environment.title} - ${artistName} - ${translation}`);
  }
}
