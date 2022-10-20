import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-general-page',
  templateUrl: './general-page.component.html',
  styleUrls: ['./general-page.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GeneralPageComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
