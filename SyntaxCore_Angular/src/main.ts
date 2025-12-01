import { platformBrowser, bootstrapApplication } from '@angular/platform-browser';
import { AppRootComponent } from './app/UI/app-root/app-root.component';
import { provideStore } from '@ngrx/store'

bootstrapApplication(AppRootComponent, {
    providers: [provideStore()]
})
  .catch(err => {
    console.log(err)
  })

