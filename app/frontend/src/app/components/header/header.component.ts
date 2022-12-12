import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzPageHeaderModule } from 'ng-zorro-antd/page-header';
import { Observable } from 'rxjs';
import { UserDto } from '../../../api/api.models';
import { ACCESS_TOKEN_COOKIE_NAME, REFRESH_TOKEN_COOKIE_NAME } from '../../common/constants';
import { deleteCookie } from '../../common/functions';
import { SettingsModalComponent } from '../settings-modal/settings-modal.component';
import { SvgIconComponent } from '../svg-icon/svg-icon.component';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, NzPageHeaderModule, NzAvatarModule, SvgIconComponent, NzDropDownModule, RouterLink, NzIconModule, SettingsModalComponent],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HeaderComponent {
  @Input() user$?: Observable<UserDto>;

  isSettingsModalOpen = false;

  constructor(private readonly router: Router) {}

  logout(): void {
    deleteCookie(ACCESS_TOKEN_COOKIE_NAME);
    deleteCookie(REFRESH_TOKEN_COOKIE_NAME);
    this.router.navigate(['auth']).then();
  }
}
