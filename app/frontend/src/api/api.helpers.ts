import { AppHttpHeaders, HttpOptions } from './api.models';
import { getCookie } from '../app/common/functions';
import { HttpResponse } from '@angular/common/http';

export class ApiException extends Error {
  override message: string;
  status: number;
  response: string;
  headers: Record<string, any>;
  result: any;

  constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
    super();

    this.message = message;
    this.status = status;
    this.response = response;
    this.headers = headers;
    this.result = result;
  }

  protected isApiException = true;

  static isApiException(obj: any): obj is ApiException {
    return obj.isApiException === true;
  }
}

export function throwException(message: string, status: number, response: string, headers: Record<string, any>, result?: any): any {
  if (result != null) {
    throw result;
  } else {
    throw new ApiException(message, status, response, headers, null);
  }
}

export function getTransformHeaders(options?: HttpOptions): AppHttpHeaders {
  const defaultHeaders = {
    Authorization: `Bearer ${getCookie('accessToken')}`,
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  };

  return options ? {...options.headers, ...defaultHeaders} : defaultHeaders;
}

export function getHandledResponse<T>(response: HttpResponse<T>): T {
  const status = response.status;
  if (status === 200 || status === 204) {
    return response.body as T;
  }
  return throwException(`${response?.body}`, status, `${response?.body}`, response.headers);
}

export function getTokens(): string {
  return JSON.stringify({
    accessToken: getCookie('accessToken'),
    refreshToken: getCookie('refreshToken')
  });
}
