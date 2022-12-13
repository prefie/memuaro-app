import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, NgZone } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { ModalButtonOptions, NzModalModule } from 'ng-zorro-antd/modal';
import { NzPageHeaderModule } from 'ng-zorro-antd/page-header';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { BehaviorSubject, Observable, Subject, switchMap, take, takeUntil } from 'rxjs';
import { UserDto } from '../../../api/api.models';
import { ApiService } from '../../../api/api.service';
import { ACCESS_TOKEN_COOKIE_NAME, REFRESH_TOKEN_COOKIE_NAME } from '../../common/constants';
import { deleteCookie } from '../../common/functions';
import { SvgIconComponent } from '../svg-icon/svg-icon.component';
import { SETTINGS_MODAL_PERIOD_OPTIONS } from './header.models';

@Component({
  selector: 'app-header',
  standalone: true,
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    CommonModule,
    NzPageHeaderModule,
    NzAvatarModule,
    SvgIconComponent,
    NzDropDownModule,
    RouterLink,
    NzIconModule,
    NzInputModule,
    NzButtonModule,
    ReactiveFormsModule,
    NzModalModule,
    NzSelectModule
  ],
})
export class HeaderComponent {
  @Input() user$?: Observable<UserDto>;

  readonly settingsForm = this.fb.group({
    periodInDays: 0,
    email: '',
    telegramName: ''
  });
  readonly modalFooter: ModalButtonOptions[]  = [{
    label: 'Сохранить',
    size: 'large',
    type: 'primary',
    onClick: () => this.saveSettings(this.settingsForm)
  }];
  readonly periodOptions = SETTINGS_MODAL_PERIOD_OPTIONS;

  readonly isSettingsModalOpen$ = new BehaviorSubject<boolean>(false);

  private readonly destroy$ = new Subject<void>();

  constructor(private readonly router: Router,
              private readonly ngZone: NgZone,
              private readonly fb: FormBuilder,
              private readonly apiService: ApiService) {}

  ngOnInit(): void {
    this.user$?.pipe(
      switchMap((user) => this.apiService.getNotificationSettings(user.id)),
      take(1),
      takeUntil(this.destroy$)
    ).subscribe((settings) => {
      if (settings) {
        this.settingsForm.patchValue(settings);
      }
    });
  }

  logout(): void {
    deleteCookie(ACCESS_TOKEN_COOKIE_NAME);
    deleteCookie(REFRESH_TOKEN_COOKIE_NAME);
    this.ngZone.run(() => this.router.navigate(['auth']).then(() => window.location.reload()));
  }

  saveSettings(form: FormGroup): void {
    this.user$?.pipe(
      switchMap((user) => this.apiService.saveNotificationSettings(user.id, form.value)),
      take(1),
      takeUntil(this.destroy$)
    ).subscribe(() => {
      this.isSettingsModalOpen$.next(false);
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
