import { SocialAuthService } from '@abacritt/angularx-social-login';
import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Subject, switchMap, take, takeUntil, tap } from 'rxjs';
import { ApiService } from '../../api/api.service';

@Component({
  selector: 'app-auth-page',
  templateUrl: './auth-page.component.html',
  styleUrls: ['./auth-page.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AuthPageComponent implements OnInit, OnDestroy {
  readonly loading$ = new BehaviorSubject<boolean>(false);

  private readonly destroy$ = new Subject<void>();

  constructor(private readonly authService: SocialAuthService,
              private readonly router: Router,
              private readonly apiService: ApiService) {}

  ngOnInit(): void {
    this.authService.authState.pipe(
      tap(() => this.loading$.next(true)),
      switchMap((user) => this.apiService.login(user.idToken)),
      take(1),
      takeUntil(this.destroy$)
    ).subscribe(() => {
      this.router.navigate(['app', 'general']).then();
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
