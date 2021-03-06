namespace a02_shopsystem.DTO
{
    public partial class ArticleUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal EuroPrice { get; set; }

        // Shop is excluded, Shop is handled via the ShopId (foreign key relating to the shop)
        // ShopId will be provided as a url parameter
    }
}
