namespace a02_shopsystem.DTO
{
    public partial class ShopDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; } = null!;

        // Articles are excluded (queryable seperately)
    }
}