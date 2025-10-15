import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Roles } from '../../models/roles';

@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
  imports: [FormsModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginComponent {
  protected readonly username = signal('');
  protected readonly selectedRole = signal<string>(Roles.Employee);
  protected readonly roles = [
    { value: Roles.Employee, label: 'Employee' },
    { value: Roles.Admin, label: 'Admin' }
  ];

  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  protected onLogin(): void {
    const username = this.username().trim();
    const role = this.selectedRole();

    if (!username) {
      return;
    }

    this.authService.login(username, role);
    this.router.navigate(['/']);
  }
}
