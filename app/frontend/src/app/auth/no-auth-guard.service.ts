import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import jwtDecode from 'jwt-decode';
import { ACCESS_TOKEN_COOKIE_NAME } from '../common/constants';
import { getCookie } from '../common/functions';
import { DecodedJwtToken } from '../common/interfaces';

@Injectable({
  providedIn: 'root'
})
export class NoAuthGuard implements CanActivate {
  constructor(private readonly router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree {
    const accessToken = getCookie(ACCESS_TOKEN_COOKIE_NAME);
    if (accessToken && (jwtDecode(accessToken) as DecodedJwtToken).exp * 1000 > Date.now()) {
      return this.router.createUrlTree(['/app/general']);
    }
    return true;
  }

}
