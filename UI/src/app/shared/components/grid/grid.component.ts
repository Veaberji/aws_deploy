import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import CardItem from '../../models/card-item';
import GridSizes from '../../models/grid-sizes';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GridComponent implements OnInit {
  breakpoint: number = 0;
  @Input() sizes: GridSizes = {
    small: 1,
    middle: 2,
    large: 3,
    extraLarge: 4,
  };
  @Input() data: CardItem[] = [];
  @Input() id: string = '';

  ngOnInit() {
    this.setBreakpoint(window.innerWidth);
  }

  onResize(event: any) {
    this.setBreakpoint(event.target.innerWidth);
  }

  private setBreakpoint(width: number) {
    if (width <= 650) {
      this.breakpoint = this.sizes.small;
    } else if (width <= 850) {
      this.breakpoint = this.sizes.middle;
    } else if (width <= 1100) {
      this.breakpoint = this.sizes.large;
    } else {
      this.breakpoint = this.sizes.extraLarge;
    }
  }
}
