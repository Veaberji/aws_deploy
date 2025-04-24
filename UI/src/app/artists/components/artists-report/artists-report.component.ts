import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { BehaviorSubject, Subscription, Observable, tap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { saveAs } from 'file-saver';
import { environment } from '../../../../environments/environment';
import { FileService } from '../../../../app/shared/services/file.service';
import { LanguageService } from 'src/app/shared/services/language.service';
import ReportRequest from '../../../../app/shared/models/report-request';

@UntilDestroy()
@Component({
  selector: 'app-artists-report',
  templateUrl: './artists-report.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ArtistsReportComponent implements OnInit, OnDestroy {
  private reportSubscription: Subscription = new Subscription();
  private isDataLoading: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  readonly maxNameLength: number = 100;
  readonly minAmount: number = 10;
  readonly maxAmount: number = 1000;
  isDataLoading$: Observable<boolean> = this.isDataLoading.asObservable();

  constructor(private titleService: Title, private fileService: FileService, private translate: LanguageService) {}

  ngOnInit(): void {
    this.initTitle();
  }

  ngOnDestroy(): void {
    this.reportSubscription.unsubscribe();
  }

  onReportRequested(request: ReportRequest): void {
    this.startLoading();
    this.reportSubscription = this.fileService.getArtistsReport(request).subscribe((file) => {
      this.stopLoading();
      saveAs(file.data, file.name);
    });
  }

  onReportCanceled(): void {
    this.reportSubscription.unsubscribe();
    this.stopLoading();
  }

  private initTitle() {
    this.translate
      .getTranslation('artistsReport.pageTitle')
      .pipe(
        tap((translation) => this.setTitle(translation)),
        untilDestroyed(this)
      )
      .subscribe();
  }

  private setTitle(translation: string) {
    this.titleService.setTitle(`${environment.title} - ${translation}`);
  }

  private startLoading() {
    this.isDataLoading.next(true);
  }

  private stopLoading() {
    this.isDataLoading.next(false);
  }
}
