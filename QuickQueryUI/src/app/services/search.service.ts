import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, WritableSignal, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { CountrySearchResult } from '../models/Country';
import { ErrorService } from './error.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  public activeCountry: WritableSignal<CountrySearchResult | undefined> 
    = signal<CountrySearchResult | undefined>(undefined);

  constructor(private http : HttpClient,
    private errorService : ErrorService,
    private router : Router
  ) { }

  searchCountry(countryName: string) {
    this.http.get<CountrySearchResult>(
      `${environment.searchServiceUrl}api/search/country?name=${countryName}`
    ).subscribe({
      next: (result: CountrySearchResult) => {
        this.activeCountry.set(result);
        this.router.navigate(['/home/country/'+result.id]);
      },
      error: (error : HttpErrorResponse) => {
        this.errorService.showError(error);
      }
    });
  }
}