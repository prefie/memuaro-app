import { ClientBase } from './client-base';

export class ApiClient extends ClientBase {
  private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
  private readonly baseUrl: string;

  constructor(baseUrl: string) {
    super();
    this.http = window as any;
    this.baseUrl = baseUrl;
  }

  login(idToken: string): Promise<TokensDto> {
    const url = `${this.baseUrl}/api/auth/login?idToken=${idToken}`;
    return this.post<TokensDto>(url);
  }

  refresh(tokens: TokensDto): Promise<TokensDto> {
    const url = `${this.baseUrl}/api/auth/refresh`;
    const content = JSON.stringify(tokens);
    return this.post<TokensDto>(url, content);
  }

  getNewQuestion(userId: string): Promise<Question> {
    const url = `${this.baseUrl}/api/questions/new?userId=${userId}`;
    return this.get<Question>(url);
  }

  getAllQuestionsForUser(userId: string): Promise<QuestionsDto> {
    const url = `${this.baseUrl}/api/questions?userId=${userId}`;
    return this.get<QuestionsDto>(url);
  }

  giveAnswer(questionId: string, answer: AnswerRequestDto): Promise<Question> {
    const url = `${this.baseUrl}/api/questions/${questionId}`;
    const content = JSON.stringify(answer);

    return this.patch<Question>(url, content);
  }

  createNewGlobalQuestion(globalQuestion: GlobalQuestionRequestDto): Promise<void> {
    const url = `${this.baseUrl}/api/questions/newGlobalQuestion`;
    const content = JSON.stringify(globalQuestion);

    return this.post<void>(url, content);
  }

  getCurrentUser(): Promise<UserDto> {
    const url = `${this.baseUrl}/api/users/current`;

    return this.get<UserDto>(url);
  }

  private get<T>(url: string) {
    const options: RequestInit = {
      method: "GET",
      headers: {
        "Accept": "application/json"
      }
    }

    return this.sendRequest<T>(url, options);
  }

  private post<T>(url: string, body: string | null = null) {
    const options: RequestInit = {
      body: body,
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        "Accept": "application/json"
      }
    };

    return this.sendRequest<T>(url, options);
  }

  private patch<T>(url: string, body: string) {
    const options: RequestInit = {
      body: body,
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
        "Accept": "application/json"
      }
    };

    return this.sendRequest<T>(url, options);
  }

  private sendRequest<T>(url: string, options: RequestInit) {
    return this.transformOptions(options)
      .then(options => this.http.fetch(url, options))
      .then((response: Response) => {
        const status = response.status;
        if (status === 200) {
          return response.json().then(json => JSON.parse(json) as T);
        } else if (status !== 200 && status !== 204) {
          return response.json().then(json => throwException(json, status, json, response.headers));
        }

        return Promise.resolve<T>(null as any);
      });
  }
}

export interface TokensDto {
  accessToken?: string;
  refreshToken?: string;
}

export interface Question {
  id: string;
  title: string;
  userId: string;
  answer?: string;
}

export interface QuestionsDto {
  questions: Question[];
}

export interface AnswerRequestDto {
  answer: string;
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

export class ApiException extends Error {
  override message: string;
  status: number;
  response: string;
  headers: { [key: string]: any; };
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

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): any {
  if (result !== null && result !== undefined)
    throw result;
  else
    throw new ApiException(message, status, response, headers, null);
}
