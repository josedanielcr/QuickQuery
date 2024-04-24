import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { LocalstorageService } from '../services/localstorage.service';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  if(req.url.endsWith('/login') || req.url.endsWith('/sign-up')) return next(req);
  
  try{
    const token = inject(LocalstorageService).getItem('token');

    if (token) {
      const reqWithHeader = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${token}`)
      });
      return next(reqWithHeader);
    }
  } catch (error) {
    console.log('Error: ', error);
  }

  return next(req);
  
};
