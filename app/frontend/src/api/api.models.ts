import { HttpContext, HttpHeaders, HttpParams } from '@angular/common/http';
import { QuestionStatus } from '../app/main/pages/general/general-page.models';

export interface HttpOptions {
  headers?: AppHttpHeaders;
  observe?: 'body' | 'response' | 'events';
  context?: HttpContext;
  params?: HttpParams | {
    [param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>;
  };
  reportProgress?: boolean;
  responseType?: 'json';
  withCredentials?: boolean;
}

export type AppHttpHeaders = HttpHeaders | {
  [header: string]: string | string[];
};

export interface TokensDto {
  accessToken: string;
  refreshToken: string;
}

export interface QuestionDto {
  id: string;
  title: string;
  userId: string;
  answer?: string;
  status: QuestionStatus;
}

export interface QuestionsDto {
  questions: QuestionDto[];
}

export interface AnswerRequestDto {
  answer: string;
  newStatus?: QuestionStatus;
}

export interface GlobalQuestionRequestDto {
  title: string;
}

export interface UserDto {
  id: string;
  email: string;
  name?: string;
  photoUrl?: string;
  roles: string[];
}
