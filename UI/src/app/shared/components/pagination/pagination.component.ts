import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import Paging from '../../models/paging';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PaginationComponent {
  @Input() pageSizes: number[] = [];
  @Input() paging$: Observable<Paging> = new Subject<Paging>();
  @Input() totalItems$: Observable<number> = new Subject<number>();
  @Output() pageChange = new EventEmitter<number>();
  @Output() pageSizeChange = new EventEmitter<number>();

  onPageChange(page: number): void {
    this.pageChange.emit(page);
  }

  onPageSizeChange(size: number): void {
    this.pageSizeChange.emit(size);
  }
}
