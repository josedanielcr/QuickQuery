export class ChartDataPoint {
    public label : string | undefined;
    public y : number | undefined;

    constructor(label : string, y : number) {
        this.label = label;
        this.y = y;
    }
}