import { Injectable } from '@angular/core';
import { LocalstorageService } from './localstorage.service';
import * as jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor(private localstorageService : LocalstorageService) { }

  isTokenValid(): boolean {
    try {
      const token = this.localstorageService.getItem('token');
      const decodedToken = jwt_decode.jwtDecode(token);

      if (decodedToken.exp === undefined) {
        return false;
      }

      const date = new Date(0); 
      date.setUTCSeconds(decodedToken.exp);
      return date.valueOf() > new Date().valueOf();
    } catch (error) {
      return false;
    }
  }

  setToken(token: string) {
    this.localstorageService.setItem('token', token);
  }
}