namespace Beauty_Works.Models.DTO.Product
{
    public class UpdateProductRequestDto
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public string? Desp { get; set; }
        public DateTime? PublishedDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public int? SubcategoryID { get; set; }
        public int? StatusID { get; set; }
        public int? BrandID { get; set; }
    }
}
