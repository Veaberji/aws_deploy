import { ErrorHandler, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { DatePipe } from '@angular/common';
import { LoggerModule } from 'ngx-logger';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { ToastrModule } from 'ngx-toastr';
import { SharedModule } from './shared/shared.module';
import { environment } from 'src/environments/environment';
import { AppErrorHandler } from './shared/common/errors/app-error-handler';
import { AppComponent } from './app.component';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [AppComponent],
  providers: [DatePipe, { provide: ErrorHandler, useClass: AppErrorHandler }],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    SharedModule,
    LoggerModule.forRoot({ ...environment.logging, timestampFormat: 'HH:mm:ss.SSS' }),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
    }),
    ToastrModule.forRoot(),
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
