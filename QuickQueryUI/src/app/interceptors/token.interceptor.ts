import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { LocalstorageService } from '../services/localstorage.service';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {

  if(req.url.endsWith('/login') || req.url.endsWith('/sign-up')) return next(req);

  const token = inject(LocalstorageService).getItem('token');

  if (token) {
    req.headers.set('Authorization', `Bearer ${token}`);
  }

  return next(req);
};
