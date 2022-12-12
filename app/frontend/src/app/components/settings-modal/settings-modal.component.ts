import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { ModalButtonOptions, NzModalComponent, NzModalModule } from 'ng-zorro-antd/modal';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { Observable, Subject, switchMap, take, takeUntil } from 'rxjs';
import { UserDto } from '../../../api/api.models';
import { ApiService } from '../../../api/api.service';
import { SelectOption } from '../../common/interfaces';
import { MainService } from '../../main/main.service';

@Component({
  selector: 'app-settings-modal',
  standalone: true,
  imports: [CommonModule, NzModalModule, ReactiveFormsModule, NzSelectModule, NzInputModule, NzButtonModule, NzIconModule],
  templateUrl: './settings-modal.component.html',
  styleUrls: ['./settings-modal.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SettingsModalComponent {
  @ViewChild(NzModalComponent) modal?: NzModalComponent;

  @Input() isOpen = false;

  @Output() modalClosed = new EventEmitter<void>();

  readonly settingsForm = this.fb.group({
    periodInDays: 0,
    email: '',
    telegramName: ''
  });
  readonly periodOptions: SelectOption<number>[] = [
    {
      value: 14,
      label: 'Две недели'
    },
    {
      value: 7,
      label: 'Неделю'
    },
    {
      value: 5,
      label: '5 дней'
    },
    {
      value: 3,
      label: '3 дня'
    },
    {
      value: 1,
      label: 'День'
    }
  ];
  readonly modalFooter: ModalButtonOptions[]  = [{
    label: 'Сохранить',
    size: 'large',
    type: 'primary',
    onClick: () => this.saveSettings(this.settingsForm)
  }];

  readonly user$: Observable<UserDto>;

  private readonly destroy$ = new Subject<void>();

  constructor(private readonly fb: FormBuilder,
              private readonly mainService: MainService,
              private readonly apiService: ApiService) {
    this.user$ = mainService.user$;
    this.user$.pipe(
      switchMap((user) => apiService.getNotificationSettings(user.id)),
      take(1),
      takeUntil(this.destroy$)
    ).subscribe((settings) => {
      if (settings) {
        this.settingsForm.patchValue(settings);
      }
    });
  }

  saveSettings(form: FormGroup): void {
    this.user$.pipe(
      switchMap((user) => this.apiService.saveNotificationSettings(user.id, form.value)),
      take(1),
      takeUntil(this.destroy$)
    ).subscribe(() => {
      this.isOpen = false;
    });
  }

  close(): void {
    this.modal?.close();
    this.modalClosed.emit();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
