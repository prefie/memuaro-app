import { Injectable } from '@angular/core';
import { GatewayClientService } from './gateway-client.service';
import {
  AnswerRequestDto,
  GlobalQuestionRequestDto,
  QuestionDto,
  QuestionsDto,
  TokensDto,
  UserDto
} from './api.models';
import { Observable, tap } from 'rxjs';
import { setCookie } from '../app/common/functions';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(private readonly gatewayClientService: GatewayClientService) {}

  login(idToken: string): Observable<TokensDto> {
    return this.gatewayClientService.post<TokensDto>('/api/auth/login', { idToken }).pipe(
      tap((tokensDto) => {
        setCookie('accessToken', tokensDto.accessToken, 10);
        setCookie('refreshToken', tokensDto.refreshToken, 10);
      })
    );
  }

  getNewQuestion(userId: string): Observable<QuestionDto> {
    return this.gatewayClientService.get<QuestionDto>('/api/questions/new', {
      params: { userId }
    });
  }

  getAllQuestionsForUser(userId: string): Observable<QuestionsDto> {
    return this.gatewayClientService.get<QuestionsDto>('/api/questions', {
      params: { userId }
    });
  }

  giveAnswer(questionId: string, answer: AnswerRequestDto): Observable<QuestionDto> {
    const content = JSON.stringify(answer);
    return this.gatewayClientService.patch<QuestionDto>(`/api/questions/${questionId}`, content);
  }

  createNewGlobalQuestion(globalQuestion: GlobalQuestionRequestDto): Observable<void> {
    const content = JSON.stringify(globalQuestion);
    return this.gatewayClientService.post<void>('/api/questions/newGlobalQuestion', content);
  }

  getCurrentUser(): Observable<UserDto> {
    return this.gatewayClientService.get<UserDto>('/api/users/current');
  }
}
