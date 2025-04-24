import { ChangeDetectionStrategy, Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { nameof } from '../../common/utils/nameof';
import TableColumn from '../../models/table-column';
import TableRow from '../../models/table-row';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TableComponent implements OnInit, OnChanges {
  @Input() columns: TableColumn[] = [];
  @Input() rows: TableRow[] = [];
  displayedColumns: Array<string> = [];

  ngOnInit(): void {
    this.initColumns();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes[nameof<TableComponent>('columns')]) {
      this.initColumns();
    }
  }

  private initColumns(): void {
    this.displayedColumns = this.columns.map((c) => c.header);
  }
}
