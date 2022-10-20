import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { SocialAuthService, SocialUser } from '@abacritt/angularx-social-login';
import { Subject, takeUntil } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-auth-page',
  templateUrl: './auth-page.component.html',
  styleUrls: ['./auth-page.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AuthPageComponent implements OnInit, OnDestroy {
  private user!: SocialUser;

  private readonly destroy$ = new Subject<void>();

  constructor(private readonly authService: SocialAuthService,
              private readonly router: Router) {}

  ngOnInit(): void {
    this.authService.authState.pipe(
      takeUntil(this.destroy$)
    ).subscribe((user) => {
      this.user = user;
      // TODO: передавать idToken на бэк
      this.router.navigate(['general']).then();
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
