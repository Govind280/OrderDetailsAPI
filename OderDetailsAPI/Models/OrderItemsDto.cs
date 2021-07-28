namespace OderDetailsAPI.Models
{
    public class OrderItemsDto
    {
        public string Product { get; set; }

        public int? Quantity { get; set; }

        public decimal? PriceEach { get; set; }
    }
}
