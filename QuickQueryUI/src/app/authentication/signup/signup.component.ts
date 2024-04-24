import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ErrorService } from '../../services/error.service';
import { Router, RouterLink } from '@angular/router';
import { AuthenticationService } from '../../services/authentication.service';
import { confirmPasswordValidator } from '../../validators/confirmPasswordValidator';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {

  signUpForm = this.formBuilder.group({
    username: ['', Validators.required],
    email: ['', Validators.required],
    password: ['', Validators.required],
    confirmPassword: ['', Validators.required]
  }, { validators: confirmPasswordValidator });

  constructor(private formBuilder : FormBuilder,
    private authenticationService : AuthenticationService,
    private errorService : ErrorService,
    private router : Router) { }

  executeSignUp() {
    if(this.signUpForm.invalid) {
      this.signUpForm.markAllAsTouched();
      return;
    }
    const { username, email, password, confirmPassword } = this.retrieveSignUpParameters();
    this.executeSignUpRequest(username, email, password, confirmPassword);
  }

  private executeSignUpRequest(username: string | null, email: string | null, password: string | null, confirmPassword: string | null) {
    this.authenticationService.signUp(username, email, password).subscribe({
      next : () => {
        this.router.navigate(['/login']);
      },
      error : (error : HttpErrorResponse) => {
        this.errorService.showError(error);
      }
    });
  }

  private retrieveSignUpParameters(): { username: string | null; email: string | null;password: string | null; 
      confirmPassword: string | null; } {
    const username = this.signUpForm.get('username')!.value;
    const email = this.signUpForm.get('email')!.value;
    const password = this.signUpForm.get('password')!.value;
    const confirmPassword = this.signUpForm.get('confirmPassword')!.value;
    return { username, email, password, confirmPassword };
  }
}
