import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthPageComponent } from './auth-page.component';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzPageHeaderModule } from 'ng-zorro-antd/page-header';
import { AuthPageRoutingModule } from './auth-page-routing.module';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { FormsModule } from '@angular/forms';
import { GoogleLoginProvider, SocialAuthServiceConfig, SocialLoginModule } from '@abacritt/angularx-social-login';
import { CLIENT_ID } from './auth-page.models';
import { SvgIconComponent } from '../components/svg-icon/svg-icon.component';
import { LoaderModule } from '../components/loader/loader.module';
import { HeaderComponent } from "../components/header/header.component";

@NgModule({
  declarations: [
    AuthPageComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    NzLayoutModule,
    NzPageHeaderModule,
    AuthPageRoutingModule,
    NzIconModule,
    NzButtonModule,
    SvgIconComponent,
    SocialLoginModule,
    LoaderModule,
    HeaderComponent
  ],
  providers: [
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider(CLIENT_ID),
          },
        ],
      } as SocialAuthServiceConfig,
    },
  ],
})
export class AuthPageModule {}
