import { Component, OnInit } from '@angular/core';
import { SearchService } from '../../services/search.service';
import { CanvasJSAngularChartsModule } from '@canvasjs/angular-charts';
import { ChartData } from '../../models/ChartData';
import { CostOfLivingChart } from '../../models/CostOfLivingChart';
import { ChartDataPoint } from '../../models/ChartDataPoint';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-country',
  standalone: true,
  imports: [
    CanvasJSAngularChartsModule,
    RouterLink
  ],
  templateUrl: './country.component.html',
  styleUrl: './country.component.css'
})
export class CountryComponent implements OnInit {

  public chartData : CostOfLivingChart | undefined;
  
  constructor(public searchService : SearchService) {}
  
  ngOnInit(): void {
    const dataPoints : ChartDataPoint[] = this.getDataPoints();
    const chartData = new ChartData('column');
    chartData.dataPoints = dataPoints;
    this.chartData = new CostOfLivingChart("Cost of Living Index",
      chartData
    );
    console.log(this.chartData);
  }

  private getDataPoints(): ChartDataPoint[] {
    const dataPoints : ChartDataPoint[] = [];
    dataPoints.push(new ChartDataPoint("Cost of living index", 
      this.searchService.activeCountry()?.costOfLivingIndex!));
    dataPoints.push(new ChartDataPoint("Rent index",
      this.searchService.activeCountry()?.rentIndex!));
    dataPoints.push(new ChartDataPoint("Groceries index",
      this.searchService.activeCountry()?.groceriesIndex!));
    dataPoints.push(new ChartDataPoint("Restaurant price index",
      this.searchService.activeCountry()?.restaurantPriceIndex!));
    dataPoints.push(new ChartDataPoint("Local purchasing power index",
      this.searchService.activeCountry()?.localPurchasingPowerIndex!));
    return dataPoints;
  }
  
}