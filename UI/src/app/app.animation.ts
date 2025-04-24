import { trigger, animate, transition, style, group, query } from '@angular/animations';

export const fadeAnimation = trigger('fadeAnimation', [
  transition('* <=> *', [
    group([
      query(':enter', [style({ opacity: 0 }), animate('300ms ease-in', style({ opacity: 1 }))], { optional: true }),
      query(':leave', [animate('300ms ease-in', style({ opacity: 0 }))], { optional: true }),
    ]),
  ]),
]);
