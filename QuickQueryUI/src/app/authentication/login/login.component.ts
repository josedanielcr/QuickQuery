import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthenticationService } from '../../services/authentication.service';
import { LoginResult } from '../../models/loginResult';
import { ErrorService } from '../../services/error.service';
import { HttpErrorResponse } from '@angular/common/http';
import { TokenService } from '../../services/token.service';
import { UserService } from '../../services/user.service';
import { LocalstorageService } from '../../services/localstorage.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  loginForm = this.formBuilder.group({
    email: ['', Validators.required],
    password: ['', Validators.required]
  });

  constructor(private formBuilder : FormBuilder,
    private authenticationService : AuthenticationService,
    private errorService : ErrorService,
    private tokenService : TokenService,
    private userService : UserService,
    private router : Router,
    private localStorageService : LocalstorageService) { }

  executeLogin() {
    const { email, password } = this.retrieveLoginParameters();
    this.executeLoginRequest(email, password);
  }

  private executeLoginRequest(email: string | null, password: string | null) {
    this.authenticationService.login(email, password).subscribe({
      next : (loginResult : LoginResult) => {
        this.manageLoginResult(loginResult);
      },
      error : (error : HttpErrorResponse) => {
        this.errorService.showError(error);
      }
    });
  }

  private manageLoginResult(loginResult: LoginResult) {
    this.tokenService.setToken(loginResult.token!);
    this.userService.setUser(loginResult.user!);
    this.localStorageService.setItem('user', loginResult.user!);
    this.router.navigate(['/']);
  }

  private retrieveLoginParameters() {
    const email = this.loginForm.get('email')!.value;
    const password = this.loginForm.get('password')!.value;
    return { email, password };
  }
}