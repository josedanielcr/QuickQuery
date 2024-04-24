import { ChartData } from "./ChartData";

export class CostOfLivingChart {
  public title : any;
  data: ChartData[] | undefined = [];

  constructor(title : string, data : ChartData) {
    this.title = {
      text: title
    };
    this.data?.push(data);
  }
}