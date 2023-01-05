import { Location } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NzNotificationService } from 'ng-zorro-antd/notification';

@Component({
  selector: 'app-book-order-form',
  templateUrl: './book-order-form.component.html',
  styleUrls: ['./book-order-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BookOrderFormComponent {
  readonly form = this.fb.group({
    region: ['', [Validators.required]],
    district: '',
    town: ['', [Validators.required]],
    street: ['', [Validators.required]],
    house: ['', [Validators.required]],
    building: '',
    flat: [0, [Validators.required]]
  });

  constructor(private readonly location: Location,
              private readonly router: Router,
              private readonly fb: FormBuilder,
              private readonly notification: NzNotificationService) {}

  back(def: string): void {
    if (history.length > 1 && history.state.innerRouting) {
      this.location.back();
    } else {
      this.router.navigateByUrl(def, {replaceUrl: true}).then();
    }
  }

  orderBook(): void {
    this.notification.create(
      'success',
      'Ваша книга успешно оформлена',
      '',
      { nzPlacement: 'bottomRight' }
    );
    this.back('/app/book-order');
  }
}
