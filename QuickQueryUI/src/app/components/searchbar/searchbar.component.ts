import { Component, ElementRef, ViewChild } from '@angular/core';
import { AutocompleteService } from '../../services/autocomplete.service';
import { NgClass } from '@angular/common';
import { SearchService } from '../../services/search.service';

@Component({
  selector: 'app-searchbar',
  standalone: true,
  imports: [NgClass],
  templateUrl: './searchbar.component.html',
  styleUrl: './searchbar.component.css'
})
export class SearchbarComponent {

  @ViewChild('searchInput') searchInput: ElementRef<HTMLInputElement> | undefined;

  constructor(public autocompleteService : AutocompleteService,
    private searchService: SearchService
  ) { }

  cleanInput(){
    if(this.searchInput){
      this.searchInput.nativeElement.value = '';
    }
    this.autocompleteService.countries.set([]);
  }

  focusInput(){
    if(this.searchInput){
      this.searchInput.nativeElement.focus();
    }
  }

  autocompleteSearch($event: Event) {
    if(this.searchInput){
      this.autocompleteService.autocompleteSearch(this.searchInput.nativeElement.value);
    }
  }

  searchCountry(countryName: string) {
    this.searchService.searchCountry(countryName);
  }
}