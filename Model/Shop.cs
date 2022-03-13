using System.Text.Json.Serialization;

namespace a02_shopsystem.Model
{
    public partial class Shop
    {
        public Shop()
        {
            Articles = new HashSet<Article>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Article> Articles { get; set; }
    }
}
