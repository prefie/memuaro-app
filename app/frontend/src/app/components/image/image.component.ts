import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, HostBinding, Input } from '@angular/core';

@Component({
  standalone: true,
  selector: 'app-image',
  templateUrl: './image.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule]
})
export class ImageComponent {
  @HostBinding('style.pointer-events') pointerEvents = 'none';
  @HostBinding('style.user-select') userSelect = 'none';
  @HostBinding('style.width') @Input() width = '100%';
  @HostBinding('style.height') @Input() height = '100%';
  @Input() alt!: string;
  @Input() imageName!: string;
}
