import { Injectable } from '@angular/core';
import { CanLoad, Route, UrlSegment } from '@angular/router';
import { map, Observable } from 'rxjs';
import { ApiService } from "../api/api.service";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanLoad {
  constructor(private readonly apiService: ApiService) {}

  canLoad(route: Route, segments: UrlSegment[]): Observable<boolean> {
    return this.apiService.getCurrentUser().pipe(
      map(() => true)
    );
  }
}
