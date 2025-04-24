import { Component, Input } from '@angular/core';
import TabItem from '../../models/tab-item';

@Component({
  selector: 'app-tab',
  templateUrl: './tab.component.html',
  styleUrls: ['./tab.component.scss'],
})
export class TabComponent {
  @Input() tabs: TabItem[] = [];
}
