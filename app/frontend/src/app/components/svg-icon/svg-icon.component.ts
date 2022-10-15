import { ChangeDetectionStrategy, Component, HostBinding, Input } from '@angular/core';

@Component({
  selector: 'app-svg-icon',
  templateUrl: './svg-icon.component.html',
  styleUrls: ['./svg-icon.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SvgIconComponent {
  @HostBinding('attr.role') role = 'img';

  @Input() icon!: string;

  spriteIconsPath = '/assets/icons/sprite.svg';
}
