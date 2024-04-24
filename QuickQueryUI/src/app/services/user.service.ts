import { Injectable, OnInit, Signal, WritableSignal, signal } from '@angular/core';
import { User } from '../models/User';
import { TokenService } from './token.service';
import { LocalstorageService } from './localstorage.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  public user :  WritableSignal<User | null> = signal<User | null>(null);

  constructor(private tokenService : TokenService,
    private localStorageService : LocalstorageService
  ) { }

  checkUserAvailability(): void {
    if(this.tokenService.isTokenValid()){
      this.user.set(this.localStorageService.getItem('user'));
    }
  }
  
  setUser(value : User){
    this.user.set(value);
  }
}