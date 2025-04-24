import { Component, ChangeDetectionStrategy, Input, OnInit } from '@angular/core';
import NavLink from '../../models/nav-link';

@Component({
  selector: 'app-main-card',
  templateUrl: './main-card.component.html',
  styleUrls: ['./main-card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MainCardComponent implements OnInit {
  textLimit = 0;
  allText = false;

  @Input() imageName: string = '';
  @Input() imageUrl: string = '';
  @Input() title: string = '';
  @Input() navLink: NavLink | undefined;
  @Input() text: string | undefined;

  ngOnInit(): void {
    this.textLimit = this.navLink ? 1300 : 1700;
  }

  onTextShowingChange() {
    this.allText = !this.allText;
  }
}
