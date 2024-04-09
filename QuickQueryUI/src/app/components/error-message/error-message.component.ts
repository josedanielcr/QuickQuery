import { Component } from '@angular/core';

@Component({
  selector: 'app-error-message',
  standalone: true,
  imports: [],
  templateUrl: './error-message.component.html',
  styleUrl: './error-message.component.css'
})
export class ErrorMessageComponent {

  private _message : string | undefined;
  private _code : string | undefined;
  private _visible : boolean | undefined;
  public title : string | undefined = 'Oops! Something went wrong!';


  constructor() { }

  public showMessage(){
    this.visible = true;
  }

  public close() {
    this.visible = false;
  }

  set message(value : string | undefined){
    this._message = value;
  }

  set code(value : string | undefined){
    this._code = value;
  }

  set visible(value : boolean | undefined){
    this._visible = value;
  }

  get visible() : boolean | undefined {
    return this._visible;
  }

  get message() : string | undefined {
    return this._message;
  }

  get code() : string | undefined {
    return this._code;
  }
}