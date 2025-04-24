import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import Paging from '../../models/paging';

@Component({
  selector: 'app-page-size-selector',
  templateUrl: './page-size-selector.component.html',
  styleUrls: ['./page-size-selector.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PageSizeSelectorComponent {
  @Input() pageSizes: number[] = [];
  @Input() paging$: Observable<Paging> = new Subject<Paging>();
  @Output() onChange = new EventEmitter<number>();

  onPageSizeChanged(size: number): void {
    this.onChange.emit(size);
  }
}
