import { Injectable, ViewContainerRef } from '@angular/core';
import { Error } from '../models/Error';
import { ErrorMessageComponent } from '../components/error-message/error-message.component';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {

  private container : ViewContainerRef | undefined;

  constructor() { }

  public registerContainer(container: ViewContainerRef | undefined) {
    this.container = container;
  }

  public getContainer() : ViewContainerRef | undefined {
    return this.container;
  }

  public showError(error : HttpErrorResponse){
    if(this.container){
      this.container.clear();
      const component : ErrorMessageComponent = 
        this.container.createComponent(ErrorMessageComponent).instance;
      component.message = error.error.message;
      component.code = error.error.code;
      component.showMessage();
    }
  }
}
