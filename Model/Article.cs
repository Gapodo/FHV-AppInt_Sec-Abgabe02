using System.Text.Json.Serialization;

namespace a02_shopsystem.Model
{
    public partial class Article
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public decimal EuroPrice { get; set; } = 0;

        [JsonIgnore]
        public int ShopId { get; set; }

        // needed to prevent loops while querying...
        [JsonIgnore]
        public virtual Shop Shop { get; set; } = null!;
    }
}
