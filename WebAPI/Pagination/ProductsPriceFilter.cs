namespace WebAPI.Pagination
{
    public class ProductsPriceFilter : QueryStringParameters
    {
        public decimal? Price { get; set; }
        public string? PriceCriterias { get; set; } // e.g., "greaterThan", "lessThan", "equalTo"
    }
}
