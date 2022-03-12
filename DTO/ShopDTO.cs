namespace a02_shopsystem.DTO
{
    public partial class ShopDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // Shop is excluded, Shop is handled via the ShopId (foreign key relating to the shop)
        // ShopId will be provided as a url parameter
    }
}
// TODO might not be needed!