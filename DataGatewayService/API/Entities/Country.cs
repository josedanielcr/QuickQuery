using API.Common;

namespace API.Entities
{
    public class Country : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public float CostOfLivingIndex { get; set; } = 0;
        public float RentIndex { get; set; } = 0;
        public float CostOfLivingPlusRentIndex { get; set; } = 0;
        public float GroceriesIndex { get; set; } = 0;
        public float RestaurantPriceIndex { get; set; } = 0;
        public float LocalPurchasingPowerIndex { get; set; } = 0;
        public int Popularity { get; set; } = 0;
    }
}