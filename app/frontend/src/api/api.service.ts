import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  AddQuestionRequestDto,
  AnswerRequestDto,
  CategoriesDto,
  CategoryDto,
  CreateGlobalQuestionRequestDto,
  GetGlobalQuestionsRequestDto,
  GlobalQuestionDto,
  GlobalQuestionsDto,
  QuestionDto,
  QuestionsDto,
  TokensDto,
  UserDto,
} from './api.models';
import { GatewayClientService } from './gateway-client.service';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(private readonly gatewayClientService: GatewayClientService) {}

  login(idToken: string): Observable<TokensDto> {
    return this.gatewayClientService.login(idToken);
  }

  getNewQuestion(userId: string, globalQuestionId: string): Observable<QuestionDto> {
    return this.gatewayClientService.get<QuestionDto>('/api/questions/new', {
      params: { userId, globalQuestionId }
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

  createNewGlobalQuestion(globalQuestion: CreateGlobalQuestionRequestDto): Observable<GlobalQuestionDto> {
    const content = JSON.stringify(globalQuestion);
    return this.gatewayClientService.post<GlobalQuestionDto>('/api/questions/newGlobalQuestion', content);
  }

  createNewQuestion(question: AddQuestionRequestDto): Observable<QuestionDto> {
    const content = JSON.stringify(question);
    return this.gatewayClientService.post<QuestionDto>('/api/questions/new', content);
  }

  getCurrentUser(): Observable<UserDto> {
    return this.gatewayClientService.get<UserDto>('/api/users/current');
  }

  getGlobalQuestions(request: GetGlobalQuestionsRequestDto): Observable<GlobalQuestionsDto> {
    return this.gatewayClientService.get<GlobalQuestionsDto>('api/questions/global', {
      params: { ...request }
    });
  }

  getAllCategories(): Observable<CategoriesDto> {
    return this.gatewayClientService.get<CategoriesDto>('/api/categories');
  }

  getCategory(categoryId: string): Observable<CategoryDto> {
    return this.gatewayClientService.get<CategoryDto>(`/api/categories/${categoryId}`);
  }
}
