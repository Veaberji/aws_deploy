import { Attribute, ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ButtonComponent {
  @Input() disabled: boolean = false;
  @Input() title: string = '';

  constructor(@Attribute('buttonId') public id: string, @Attribute('type') public type: string) {}
}
