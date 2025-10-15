import { Injectable, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly STORAGE_USERNAME_KEY = 'workshop_username';
  private readonly STORAGE_ROLE_KEY = 'workshop_role';

  private readonly usernameSignal = signal<string | null>(null);
  private readonly roleSignal = signal<string | null>(null);

  readonly username = this.usernameSignal.asReadonly();
  readonly role = this.roleSignal.asReadonly();
  readonly isAuthenticated = computed(() => 
    !!this.usernameSignal() && !!this.roleSignal()
  );

  private router = inject(Router);

  constructor() {
    this.initializeFromStorage();
  }

  private initializeFromStorage(): void {
    const username = localStorage.getItem(this.STORAGE_USERNAME_KEY);
    const role = localStorage.getItem(this.STORAGE_ROLE_KEY);
    
    if (username && role) {
      this.usernameSignal.set(username);
      this.roleSignal.set(role);
    }
  }

  login(username: string, role: string): void {
    this.usernameSignal.set(username);
    this.roleSignal.set(role);
    
    localStorage.setItem(this.STORAGE_USERNAME_KEY, username);
    localStorage.setItem(this.STORAGE_ROLE_KEY, role);
  }

  logout(): void {
    this.usernameSignal.set(null);
    this.roleSignal.set(null);
    
    localStorage.removeItem(this.STORAGE_USERNAME_KEY);
    localStorage.removeItem(this.STORAGE_ROLE_KEY);
    
    this.router.navigate(['/login']);
  }

  switchRole(newRole: string): void {
    this.roleSignal.set(newRole);
    localStorage.setItem(this.STORAGE_ROLE_KEY, newRole);
  }

  getAuthHeaders(): Record<string, string> {
    const username = this.usernameSignal();
    const role = this.roleSignal();
    
    if (!username || !role) {
      return {};
    }
    
    return {
      'X-Username': username,
      'X-Role': role
    };
  }
}
