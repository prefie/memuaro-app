import { ChangeDetectionStrategy, Component, OnDestroy } from '@angular/core';
import { BehaviorSubject, Subject, take, takeUntil } from "rxjs";
import { ApiService } from "../../api/api.service";

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdminComponent implements OnDestroy {
  newQuestion = '';

  readonly loading$ = new BehaviorSubject<boolean>(false);

  private readonly destroy$ = new Subject<void>();

  constructor(private readonly apiService: ApiService) {}

  addNewQuestion(): void {
    const title = this.newQuestion.trim();
    if (title) {
      this.loading$.next(true);
      this.apiService.createNewGlobalQuestion({ title }).pipe(
        take(1),
        takeUntil(this.destroy$)
      ).subscribe(() => {
        this.newQuestion = '';
        this.loading$.next(false);
      });
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
