import { ChartDataPoint } from "./ChartDataPoint";

export class ChartData {
  public type : string | undefined;
  public dataPoints : ChartDataPoint[] | undefined;

  constructor(type : string) {
    this.type = type;
  }
  
  addDataPoint(dataPoint : ChartDataPoint) {
    this.dataPoints?.push(dataPoint);
  }
}