using System;
using System.Collections.Generic;

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

        public virtual ICollection<Article> Articles { get; set; }
    }
}
