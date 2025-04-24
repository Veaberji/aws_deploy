import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FooterComponent implements OnInit {
  @Input() title: string = '';
  currentYear: number | undefined;

  ngOnInit(): void {
    this.currentYear = new Date().getFullYear();
  }
}
