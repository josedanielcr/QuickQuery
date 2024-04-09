import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from '../../models/User';
import { AuthenticationService } from '../../services/authentication.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit{

  constructor(public userService : UserService,
    private authenticationService : AuthenticationService
  ) { }

  ngOnInit(): void {
    this.userService.checkUserAvailability();
  }

  public signOut(): void {
    this.authenticationService.signOut();
  }

}
