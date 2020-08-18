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
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionItem> TransactionItems { get; set; }

        public MyContext() : base("BelajarCRUDWPF")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //One-to-one Item-to-Supplier
            modelBuilder.Entity<Item>()
                .HasRequired<Supplier>(S => S.Supplier)
                .WithMany(S => S.Item)
                .HasForeignKey<int>(I => I.SupplierId);

            //One-to-many Item-to-Supplier
            modelBuilder.Entity<Supplier>()
                .HasMany<Item>(s => s.Item)
                .WithRequired(i => i.Supplier)
                .HasForeignKey<int>(i => i.SupplierId)
                .WillCascadeOnDelete();

            //One-to-one TransactionItem-to-Transaction
            modelBuilder.Entity<TransactionItem>()
               .HasRequired<Transaction>(ti => ti.Transaction)
               .WithMany(t => t.TransactionItem)
               .HasForeignKey<int>(t => t.TransactionId);

            //One-to-many Transaction-to-TransactionItem
            modelBuilder.Entity<Transaction>()
                .HasMany<TransactionItem>(t => t.TransactionItem)
                .WithRequired(ti => ti.Transaction)
                .HasForeignKey<int>(ti => ti.TransactionId)
                .WillCascadeOnDelete();

            //One-to-many Item-to-TransactionItem
            modelBuilder.Entity<Item>()
                .HasMany<TransactionItem>(i => i.TransactionItems)
                .WithRequired(ti => ti.Item)
                .HasForeignKey<int>(ti => ti.ItemId)
                .WillCascadeOnDelete();
        }
    }
}
