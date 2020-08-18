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
           //Item ke supplier (one to one)
            modelBuilder.Entity<Item>()
                .HasRequired<Supplier>(S => S.Supplier)
                .WithMany(S => S.Item).HasForeignKey<int>(I => I.SupplierId);

           //item to supplier (one to many)
            modelBuilder.Entity<Supplier>()
                .HasMany<Item>(s => s.Item)
                .WithRequired(i => i.Supplier)
                .HasForeignKey<int>(i => i.SupplierId)
                .WillCascadeOnDelete();

            //TransactionItem ke Transaction (one to one)
            modelBuilder.Entity<TransactionItem>()
               .HasRequired<Transaction>(ti => ti.Transaction)
               .WithMany(ti => ti.TransactionItem).HasForeignKey<int>(t => t.TransactionId);
           
           //Transaction ke TransactionItem (one to many)
            modelBuilder.Entity<Transaction>()
                .HasMany<TransactionItem>(t => t.TransactionItem)
                .WithRequired(ti => ti.Transaction)
                .HasForeignKey<int>(ti => ti.TransactionId)
                .WillCascadeOnDelete();

            //Item ke TransactionItem (one to many)
            modelBuilder.Entity<Item>()
                .HasMany<TransactionItem>(i => i.TransactionItems)
                .WithRequired(ti => ti.Item)
                .HasForeignKey<int>(ti => ti.ItemId)
                .WillCascadeOnDelete();
        }
    }
}
