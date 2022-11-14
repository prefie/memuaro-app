import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { NzInputModule } from "ng-zorro-antd/input";
import { NzButtonModule } from "ng-zorro-antd/button";
import { LoaderModule } from "../components/loader/loader.module";
import { FormsModule } from "@angular/forms";


@NgModule({
  declarations: [
    AdminComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    NzInputModule,
    NzButtonModule,
    LoaderModule,
    FormsModule
  ]
})
export class AdminModule {}
