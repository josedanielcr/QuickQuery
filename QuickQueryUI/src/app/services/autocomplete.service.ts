import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, WritableSignal, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { ErrorService } from './error.service';

@Injectable({
  providedIn: 'root'
})
export class AutocompleteService {

  public countries : WritableSignal<string[] | undefined> = signal<string[]>([]);

  constructor(private http : HttpClient,
    private errorService : ErrorService
  ) { }

  public autocompleteSearch(prefix : string) {
    if(prefix.trim().length === 0) {
      this.countries.set([]);
      return;
    }
    this.http.get<string[]>(
      `${environment.autocompleteServiceUrl}api/autocomplete/?prefix=${prefix}`
    ).subscribe({
      next : (countries : string[]) => {
        console.log(countries);
        this.countries.set(countries);
      },
      error : (error : HttpErrorResponse) => this.errorService.showError(error) 
    })
  }
}