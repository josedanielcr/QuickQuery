export class CountrySearchResult {
    public id: string | undefined; 
    public name: string | undefined;
    public costOfLivingIndex: number | undefined;
    public rentIndex: number | undefined;
    public costOfLivingPlusRentIndex: number | undefined;
    public groceriesIndex: number | undefined;
    public restaurantPriceIndex: number | undefined;
    public localPurchasingPowerIndex: number | undefined;
    public createdAt: Date | undefined;
    public updatedAt: Date | undefined;
    public popularity: number | undefined;
}
