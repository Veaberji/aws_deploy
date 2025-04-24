import { Component, OnInit } from '@angular/core';
import { Router, Event, NavigationStart, NavigationEnd, NavigationError, NavigationCancel } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { environment } from './../environments/environment';
import { fadeAnimation } from './app.animation';
import { ThemeService } from './shared/services/theme.service';
import { LoggerService } from './shared/services/logger.service';
import { LanguageService } from './shared/services/language.service';
import { Language } from './shared/models/language';

@UntilDestroy()
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  animations: [fadeAnimation],
})
export class AppComponent implements OnInit {
  private theme = '';
  currentLanguage$: Observable<Language> = new Observable();
  title = environment.title;
  loading = true;

  constructor(
    private router: Router,
    private themeService: ThemeService,
    private logger: LoggerService,
    private translate: LanguageService
  ) {}

  ngOnInit(): void {
    this.initRouterEvent();
    this.initCurrentLanguageStream();
    this.initTheme();
  }

  onThemeChange(): void {
    this.themeService.changeBaseTheme();
  }

  onTimeBasedThemeChange(turnedOn: boolean): void {
    this.themeService.switchTimeBasedTheme(turnedOn);
  }

  onLanguageChange(language: Language): void {
    this.translate.use(language);
  }

  private initCurrentLanguageStream() {
    this.currentLanguage$ = this.translate.currentLanguage$;
  }

  private checkRouterEvent(routerEvent: Event): void {
    if (routerEvent instanceof NavigationStart) {
      this.logger.info('Navigate to:', routerEvent.url);
      this.loading = true;
    } else if (
      routerEvent instanceof NavigationEnd ||
      routerEvent instanceof NavigationCancel ||
      routerEvent instanceof NavigationError
    ) {
      this.loading = false;
    }
  }

  private initTheme() {
    this.themeService.theme$
      .pipe(
        tap((theme) => {
          if (this.theme) {
            document.body.classList.remove(this.theme);
          }
          this.theme = theme;
          document.body.classList.add(this.theme);
        }),
        untilDestroyed(this)
      )
      .subscribe();
  }

  private initRouterEvent(): void {
    this.router.events.subscribe((routerEvent: Event) => this.checkRouterEvent(routerEvent));
  }
}
