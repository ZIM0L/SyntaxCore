import { Component } from '@angular/core';
import { MainConsoleComponent } from '../main-console/main-console.component'
import { IntroPageComponent } from '../intro-page/intro-page.component';
import { Store } from '@ngrx/store';
import { initializeAppState } from '../../Services/app-state';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app-root.component.html',
  styleUrl: '../../../sass/global.css',
  imports: [MainConsoleComponent, IntroPageComponent]
})
export class AppRootComponent {

  constructor(private store: Store) { }

  ngOnInit() {
    this.store.dispatch(initializeAppState())
  }
}
