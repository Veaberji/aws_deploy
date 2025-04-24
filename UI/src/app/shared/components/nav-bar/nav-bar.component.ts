import { ChangeDetectionStrategy, Component, EventEmitter, Output } from '@angular/core';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { LanguageService } from '../../services/language.service';
import { Language } from '../../models/language';
import LanguageDisplay from '../../models/language-display';
import { ThemeService } from '../../services/theme.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NavBarComponent {
  readonly isProduction = environment.production;
  readonly currentLanguage$: Observable<Language> = this.translate.currentLanguage$;
  readonly languages: LanguageDisplay[] = this.translate.languages;
  readonly timeBasedThemeUsed$: Observable<boolean> = this.themeService.timeBasedThemeUsed$;
  @Output() themeChange = new EventEmitter();
  @Output() timeBasedThemeChange = new EventEmitter<boolean>();
  @Output() languageChange = new EventEmitter<Language>();

  constructor(private router: Router, private themeService: ThemeService, public translate: LanguageService) {}

  async reload(url: string): Promise<boolean> {
    await this.router.navigateByUrl('', { skipLocationChange: true });
    return this.router.navigateByUrl(url);
  }

  onThemeChange(): void {
    this.themeChange.emit();
  }

  onLanguageChange(language: Language): void {
    this.languageChange.emit(language);
  }

  onTimeBasedThemeChange(event: MatSlideToggleChange): void {
    this.timeBasedThemeChange.emit(event.source.checked);
  }
}
