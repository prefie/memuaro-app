import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GeneralPageComponent } from './general-page.component';
import { GeneralPageRoutingModule } from './general-page-routing.module';

@NgModule({
  declarations: [
    GeneralPageComponent
  ],
  imports: [
    CommonModule,
    GeneralPageRoutingModule
  ]
})
export class GeneralPageModule { }
