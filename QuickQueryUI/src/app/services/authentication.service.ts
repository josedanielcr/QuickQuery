import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginResult } from '../models/loginResult';
import { environment } from '../../environments/environment';
import { User } from '../models/User';
import { Router } from '@angular/router';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private http : HttpClient, 
    private router : Router,
    private userService : UserService) { }

  public login(email: string | null, password: string | null) : Observable<LoginResult> {
    return this.http.post<LoginResult>(`${environment.authenticationServiceUrl}api/login`, 
    { email, password });
  }

  public signUp(username: string | null, email: string | null, password: string | null) 
    : Observable<User> {
    return this.http.post<User>(`${environment.authenticationServiceUrl}api/signup`, 
    { username, email, password });
  }

  public getUserByEmailAddress(email: string | null) : Observable<User> {
    return this.http.get<User>(`${environment.authenticationServiceUrl}api/users/${email}`);
  }

  public signOut() : void {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    this.router.navigate(['/login']);
    this.userService.user.set(null);
  }
}