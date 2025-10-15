import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './services/auth.service';
import { RoleSwitcherComponent } from './components/role-switcher/role-switcher';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RoleSwitcherComponent],
  templateUrl: './app.html',
  styleUrl: './app.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class App {
  protected readonly authService = inject(AuthService);
}
