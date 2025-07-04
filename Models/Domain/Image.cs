namespace Beauty_Works.Models.Domain
{
    public class Image
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
