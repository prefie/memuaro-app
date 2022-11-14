import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { catchError, filter, map, Observable, of, switchMap, tap } from 'rxjs';
import { HttpOptions, TokensDto } from './api.models';
import { getHandledResponse, getTokens, getTransformHeaders } from './api.helpers';
import { Router } from '@angular/router';
import { getCookie, setCookie } from '../app/common/functions';
import jwtDecode from "jwt-decode";
import { DecodedJwtToken } from "../app/common/interfaces";

@Injectable({
  providedIn: 'root'
})
export class GatewayClientService {
  constructor(private readonly http: HttpClient,
              private readonly router: Router) {
  }

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

  private getRequest<T>(request: Observable<HttpResponse<T>>): Observable<T> {
    const accessToken = getCookie('accessToken');

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
          setCookie('accessToken', tokensDto.accessToken, 10);
          setCookie('refreshToken', tokensDto.refreshToken, 10);
        }
      })
    );
  }
}
