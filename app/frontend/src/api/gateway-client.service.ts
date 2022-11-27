import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import jwtDecode from 'jwt-decode';
import { catchError, filter, map, Observable, of, switchMap, tap } from 'rxjs';
import { ACCESS_TOKEN_COOKIE_NAME, REFRESH_TOKEN_COOKIE_NAME } from '../app/common/constants';
import { getCookie, setCookie } from '../app/common/functions';
import { DecodedJwtToken } from '../app/common/interfaces';
import { getHandledResponse, getTokens, getTransformHeaders } from './api.helpers';
import { HttpOptions, TokensDto } from './api.models';

@Injectable({
  providedIn: 'root'
})
export class GatewayClientService {
  constructor(private readonly http: HttpClient,
              private readonly router: Router) {}

  get<T>(commandUrl: string, options?: HttpOptions): Observable<T> {
    const request: Observable<HttpResponse<T>> = this.http.get<T>(commandUrl, {
      ...options,
      observe: 'response',
      headers: getTransformHeaders(options)
    });

    return this.getRequest(request);
  }

  post<T>(commandUrl: string, body?: any | null, options?: HttpOptions): Observable<T> {
    const request: Observable<HttpResponse<T>> = this.http.post<T>(commandUrl, body, {
      ...options,
      observe: 'response',
      headers: getTransformHeaders(options)
    });

    return this.getRequest(request);
  }

  patch<T>(commandUrl: string, body: any | null, options?: HttpOptions): Observable<T> {
    const request: Observable<HttpResponse<T>> = this.http.patch<T>(commandUrl, body, {
      ...options,
      observe: 'response',
      headers: getTransformHeaders(options)
    });

    return this.getRequest(request);
  }

  login(idToken: string): Observable<TokensDto> {
    return this.http.post<TokensDto>('/api/auth/login', { idToken }, {
      observe: 'response',
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
      }
    }).pipe(
      catchError(() => of(null)),
      filter((response): response is HttpResponse<TokensDto> => !!response),
      map((response) => getHandledResponse(response)),
      tap((tokensDto) => {
        setCookie(ACCESS_TOKEN_COOKIE_NAME, tokensDto.accessToken, 10);
        setCookie(REFRESH_TOKEN_COOKIE_NAME, tokensDto.refreshToken, 10);
      })
    );
  }

  private getRequest<T>(request: Observable<HttpResponse<T>>): Observable<T> {
    const accessToken = getCookie(ACCESS_TOKEN_COOKIE_NAME);

    if (!accessToken || (jwtDecode(accessToken) as DecodedJwtToken).exp * 1000 > Date.now()) {
      return request.pipe(
        catchError(() => of(null)),
        filter((response): response is HttpResponse<T> => !!response),
        map((response) => getHandledResponse(response))
      );
    }

    return this.refreshToken().pipe(
      switchMap((result) => result ? request : of(null)),
      filter((response): response is HttpResponse<T> => !!response),
      map((response) => getHandledResponse(response))
    );
  }

  private refreshToken<T>(): Observable<TokensDto | null> {
    return this.http.post<TokensDto>('/api/auth/refresh', getTokens(), {
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
      }
    }).pipe(
      catchError(() => {
        this.router.navigate(['/auth']).then();
        return of(null);
      }),
      tap((tokensDto: TokensDto | null) => {
        if (tokensDto) {
          setCookie(ACCESS_TOKEN_COOKIE_NAME, tokensDto.accessToken, 10);
          setCookie(REFRESH_TOKEN_COOKIE_NAME, tokensDto.refreshToken, 10);
        }
      })
    );
  }
}