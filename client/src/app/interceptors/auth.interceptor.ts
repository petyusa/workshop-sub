import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const authHeaders = authService.getAuthHeaders();
  
  // Only add headers if user is authenticated
  if (Object.keys(authHeaders).length === 0) {
    return next(req);
  }
  
  // Clone the request and add authentication headers
  const authReq = req.clone({
    setHeaders: authHeaders
  });
  
  return next(authReq);
};
