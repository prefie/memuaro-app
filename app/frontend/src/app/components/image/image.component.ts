import { ChangeDetectionStrategy, Component, HostBinding, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-image',
  templateUrl: './image.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule]
})
export class ImageComponent {
  @HostBinding('style.width') @Input() width = '100%';
  @HostBinding('style.height') @Input() height = '100%';
  @Input() alt!: string;
  @Input() imageName!: string;
}
