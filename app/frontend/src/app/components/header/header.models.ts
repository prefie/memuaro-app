import { SelectOption } from '../../common/interfaces';

export const SETTINGS_MODAL_PERIOD_OPTIONS: SelectOption<number>[] = [
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
