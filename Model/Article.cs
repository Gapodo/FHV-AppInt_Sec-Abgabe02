using Microsoft.EntityFrameworkCore;

namespace a02_shopsystem.Model
{
    public partial class Article
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public decimal EuroPrice { get; set; } = 0;

        public int ShopId { get; set; }

        public virtual Shop Shop { get; set; } = null!;
    }
}
