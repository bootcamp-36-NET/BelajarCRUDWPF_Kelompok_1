using BelajarCRUDWPF.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelajarCRUDWPF.Context
{
    class MyContext : DbContext
    {
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Item> Items { get; set; }

        public MyContext() : base("BelajarCRUDWPF")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasRequired<Supplier>(S => S.Supplier)
                .WithMany(S => S.Item).HasForeignKey<int>(I => I.SupplierId);

            modelBuilder.Entity<Supplier>()
                .HasMany<Item>(s => s.Item)
                .WithRequired(i => i.Supplier)
                .HasForeignKey<int>(i => i.SupplierId)
                .WillCascadeOnDelete();
        }
    }
}
