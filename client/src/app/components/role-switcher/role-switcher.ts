import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Roles } from '../../models/roles';

@Component({
  selector: 'app-role-switcher',
  templateUrl: './role-switcher.html',
  styleUrls: ['./role-switcher.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RoleSwitcherComponent {
  protected readonly authService = inject(AuthService);
  protected readonly Roles = Roles;

  protected toggleRole(): void {
    const currentRole = this.authService.role();
    const newRole = currentRole === Roles.Employee ? Roles.Admin : Roles.Employee;
    this.authService.switchRole(newRole);
  }

  protected logout(): void {
    this.authService.logout();
  }
}
