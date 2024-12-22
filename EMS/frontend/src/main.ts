import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { tokenInterceptor } from './app/interceptor/token.interceptor';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';

const config = {
  providers: [
    provideHttpClient(withInterceptors([tokenInterceptor])),
    provideRouter(routes),
  ],
};
bootstrapApplication(AppComponent, config).catch((err) => console.error(err));
