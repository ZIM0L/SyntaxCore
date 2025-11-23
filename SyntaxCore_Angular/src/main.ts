import { platformBrowser, bootstrapApplication } from '@angular/platform-browser';
import { AppRootComponent } from './app/app-root/app-root.component'

bootstrapApplication(AppRootComponent)
  .catch(err => {
    console.log(err)
  })

