using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace NongSanShop.Models
{
    public partial class NongSanDB : DbContext
    {
        public NongSanDB()
            : base("name=NongSanDB")
        {
        }

        public virtual DbSet<dh_blog> dh_blog { get; set; }
        public virtual DbSet<dh_cart> dh_cart { get; set; }
        public virtual DbSet<dh_category> dh_category { get; set; }
        public virtual DbSet<dh_order> dh_order { get; set; }
        public virtual DbSet<dh_order_product> dh_order_product { get; set; }
        public virtual DbSet<dh_product> dh_product { get; set; }
        public virtual DbSet<dh_user> dh_user { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<dh_blog>()
                .Property(e => e.thumbnail)
                .IsUnicode(false);

            modelBuilder.Entity<dh_category>()
                .HasMany(e => e.dh_product)
                .WithOptional(e => e.dh_category)
                .HasForeignKey(e => e.category_id);

            modelBuilder.Entity<dh_order>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<dh_order>()
                .Property(e => e.code_name)
                .IsUnicode(false);

            modelBuilder.Entity<dh_order>()
                .HasMany(e => e.dh_order_product)
                .WithOptional(e => e.dh_order)
                .HasForeignKey(e => e.order_id);

            modelBuilder.Entity<dh_product>()
                .HasMany(e => e.dh_cart)
                .WithOptional(e => e.dh_product)
                .HasForeignKey(e => e.product_id);

            modelBuilder.Entity<dh_product>()
                .HasMany(e => e.dh_order_product)
                .WithOptional(e => e.dh_product)
                .HasForeignKey(e => e.product_id);

            modelBuilder.Entity<dh_user>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<dh_user>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<dh_user>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<dh_user>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<dh_user>()
                .HasMany(e => e.dh_cart)
                .WithOptional(e => e.dh_user)
                .HasForeignKey(e => e.user_id);

            modelBuilder.Entity<dh_user>()
                .HasMany(e => e.dh_order)
                .WithOptional(e => e.dh_user)
                .HasForeignKey(e => e.user_id);
        }
    }
}
