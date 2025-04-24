import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import CardItem from '../../models/card-item';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CardComponent {
  @Input() data: CardItem = {
    title: '',
    imageUrl: '',
    navUrl: '',
  };

  constructor(private router: Router) {}

  goToPage(): void {
    if (!this.data) {
      return;
    }
    if (this.data.newWindow) {
      window.open(this.data.navUrl, '_blank');
    } else {
      this.router.navigate([this.data.navUrl]);
    }
  }
}
