import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { TokenService } from '../services/token.service';

export const authGuard: CanActivateFn = (route, state) => {
  if(inject(TokenService).isTokenValid()) {
    return true;
  } else {
    inject(Router).navigate(['/login']);
    return false;
  }
};