import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './authentication/login/login.component';
import { SignupComponent } from './authentication/signup/signup.component';
import { authGuard } from './guards/auth.guard';
import { CountryComponent } from './pages/country/country.component';

export const routes: Routes = [
    { path: 'home', component: HomeComponent, canActivate: [authGuard] },
    { path: 'home/country/:id', component: CountryComponent, canActivate: [authGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'sign-up', component: SignupComponent},
    { path: '**', redirectTo: 'home' }
];