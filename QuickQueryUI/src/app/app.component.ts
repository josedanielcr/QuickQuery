import { AfterViewInit, Component, OnInit, ViewChild, ViewContainerRef, viewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './components/navbar/navbar.component';
import { ErrorService } from './services/error.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, NavbarComponent],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements AfterViewInit {
  
  @ViewChild('errorContainer', { read: ViewContainerRef })
    container: ViewContainerRef | undefined;

  constructor(private errorService : ErrorService) { }

  ngAfterViewInit(): void {
    this.errorService.registerContainer(this.container);
  }
}
