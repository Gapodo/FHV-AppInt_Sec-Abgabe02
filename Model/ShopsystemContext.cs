using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace a02_shopsystem.Model
{
    public partial class ShopsystemContext : DbContext
    {
        public ShopsystemContext()
        {
        }

        public ShopsystemContext(DbContextOptions<ShopsystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Shop> Shops { get; set; } = null!;
        public virtual DbSet<Article> Articles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // charset & collation matching our db-setup and needs
            modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8");


            modelBuilder.Entity<Shop>(entity =>
            {

                entity.ToTable("Shops");

                // the primary key is implicitly indexed
                // (would need to index it elseway as we access it by the primary key (id))
                entity.HasKey(e => e.Id).HasName("PRIMARY");
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(128)
                    .HasColumnName("name");

                // indexing of the name allows for quick text-searches allowing for "search by name" scenarios
                entity.HasIndex(e => e.Name, "Shops_name_idx");
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.ToTable("Articles");

                // the primary key is implicitly indexed
                // (would need to index it elseway as we access it by the primary key (id))
                entity.HasKey(e => e.Id).HasName("PRIMARY");
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .HasColumnName("name");

                // indexing of the name allows for quick text-searches allowing for "search by name" scenarios
                entity.HasIndex(e => e.Name, "Articles_name_idx");

                // foreign key constraints should always be indexed (most SQL implementations enforce this)
                entity.HasIndex(e => e.ShopId, "Articles_shopId_fk");
                entity.Property(e => e.ShopId).HasColumnName("shopId");
                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.ShopId)
                    .HasConstraintName("Articles_shopId_fk")
                    .IsRequired();

                // not indexing since searching on numbers is inexpensive
                // (could be indexed if we intend to sort by price / below x fairly often)
                entity.Property(e => e.EuroPrice).HasColumnName("euroPrice");
                
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
