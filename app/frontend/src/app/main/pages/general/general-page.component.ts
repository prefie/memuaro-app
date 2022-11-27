import { ChangeDetectionStrategy, Component, OnDestroy } from '@angular/core';
import { ApiService } from '../../../../api/api.service';
import { MainService } from '../../main.service';
import {
  BehaviorSubject,
  map,
  Observable,
  shareReplay,
  startWith,
  Subject,
  switchMap,
  take,
  takeUntil,
  tap
} from 'rxjs';
import { QuestionDto, UserDto } from '../../../../api/api.models';
import { ANGULAR_EDITOR_CONFIG_DEFAULT, QuestionStatus } from './general-page.models';
import { NzNotificationService } from 'ng-zorro-antd/notification';

@Component({
  selector: 'app-general-page',
  templateUrl: './general-page.component.html',
  styleUrls: ['./general-page.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GeneralPageComponent implements OnDestroy {
  searchRequest = '';

  readonly config = ANGULAR_EDITOR_CONFIG_DEFAULT;
  readonly questionStatusEnum = QuestionStatus;

  readonly update$ = new Subject<void>();
  readonly user$: Observable<UserDto>;
  readonly loading$ = new BehaviorSubject<boolean>(false);
  readonly questions$: Observable<Record<QuestionStatus, QuestionDto[]>>;
  readonly activeQuestion$ = new BehaviorSubject<QuestionDto | null>(null);
  readonly isActiveQuestionPartlyAnswered$ = new BehaviorSubject<boolean>(false);

  private readonly destroy$ = new Subject<void>();

  constructor(private readonly apiService: ApiService,
              private readonly mainService: MainService,
              private readonly notification: NzNotificationService) {
    this.user$ = mainService.user$;

    this.questions$ = this.update$.pipe(
      startWith(0),
      tap(() => this.loading$.next(true)),
      switchMap(() => this.user$),
      switchMap((user) => apiService.getAllQuestionsForUser(user.id)),
      map(({questions}) => {
        const searchRequest = this.searchRequest.toLowerCase().trim();
        return questions.reduce<Record<QuestionStatus, QuestionDto[]>>((questionsByStatus, question) => {
          if (question.title.toLowerCase().includes(searchRequest)) {
            questionsByStatus[question.status ? question.status as QuestionStatus : QuestionStatus.UNANSWERED].push(question);
          }
          return questionsByStatus;
        }, {
          [QuestionStatus.UNANSWERED]: [],
          [QuestionStatus.PARTLY_ANSWERED]: [],
          [QuestionStatus.ANSWERED]: []
        });
      }),
      tap(() => this.loading$.next(false)),
      shareReplay({bufferSize: 1, refCount: true})
    );
  }

  getNewQuestion(): void {
    this.user$.pipe(
      tap(() => this.loading$.next(true)),
      switchMap((user) => this.apiService.getNewQuestion(user.id, "")),
      take(1),
      takeUntil(this.destroy$)
    ).subscribe(() => this.update$.next());
  }

  selectAsActive(question: QuestionDto): void {
    this.isActiveQuestionPartlyAnswered$.next(question.status === QuestionStatus.PARTLY_ANSWERED);
    this.activeQuestion$.next(question);
  }

  saveAnswer(question: QuestionDto): void {
    const newStatus = this.isActiveQuestionPartlyAnswered$.getValue() ? QuestionStatus.PARTLY_ANSWERED : QuestionStatus.ANSWERED;
    this.loading$.next(true);
    this.apiService.giveAnswer(question.id, {answer: `${question.answer}`, newStatus}).pipe(
      take(1),
      takeUntil(this.destroy$)
    ).subscribe(() => {
      this.update$.next();
      this.notification.create(
        'success',
        'Ваш ответ успешно сохранён',
        '',
        {nzPlacement: 'bottomRight'}
      );
      this.activeQuestion$.next(null);
    });
  }

  filterQuestionsByRequest(): void {
    this.update$.next();
    this.activeQuestion$.next(null);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
