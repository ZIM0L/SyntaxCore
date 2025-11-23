import { Component } from '@angular/core';
import { MainConsoleComponent } from '../main-console/main-console.component'

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app-root.component.html',
  styleUrl: './app-root.component.css',
  imports: [MainConsoleComponent]
})
export class AppRootComponent {

}
