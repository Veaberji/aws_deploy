import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { combineLatest, Observable, concatMap, of } from 'rxjs';
import { PagingService } from '../../services/paging.service';
import PagingDetails from '../../models/paging-details';
import Paging from '../../models/paging';
import PageSelectorData from '../../models/page-selector-data';

@Component({
  selector: 'app-page-selector',
  templateUrl: './page-selector.component.html',
  styleUrls: ['./page-selector.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageSelectorComponent implements OnInit {
  readonly pagesAmount: number = 10;
  private firstPage: number = 0;
  private lastPage: number = 0;
  pageData$: Observable<PageSelectorData> = new Observable<PageSelectorData>();

  @Input() paging$: Observable<Paging> = new Observable<Paging>();
  @Input() totalItems$: Observable<number> = new Observable<number>();
  @Output() onChange = new EventEmitter<number>();

  constructor(private service: PagingService) {}

  ngOnInit(): void {
    this.initPageDataStream();
  }

  onPageChanged(page: number): void {
    const outOfRange = page <= this.firstPage - 1 || page >= this.lastPage + 1;
    if (outOfRange) {
      return;
    }

    this.onChange.emit(page);
  }

  private initPageDataStream(): void {
    this.pageData$ = combineLatest([this.paging$, this.totalItems$]).pipe(
      concatMap(([paging, totalItems]) => {
        const pages = this.service.getPagesArray(this.getPagingDetails(paging.page, paging.pageSize, totalItems));
        this.firstPage = pages[0];
        this.lastPage = pages[pages.length - 1];
        return of({
          currentPage: paging.page,
          firstPage: this.firstPage,
          lastPage: this.lastPage,
          pageSize: paging.pageSize,
          pages: pages,
        });
      })
    );
  }

  private getPagingDetails(page: number, pageSize: number, totalItems: number): PagingDetails {
    const pagingDetails: PagingDetails = {
      CurrentPage: page,
      TotalItems: totalItems,
      PageSize: pageSize,
      PagesAmount: this.pagesAmount,
    };

    return pagingDetails;
  }
}
