import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LanguageService } from '../../services/language.service';

@UntilDestroy()
@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NotFoundComponent implements OnInit {
  constructor(private titleService: Title, private translate: LanguageService) {}

  ngOnInit(): void {
    this.initTitle();
  }

  private initTitle() {
    this.translate
      .getTranslation('notFound.pageTitle')
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
