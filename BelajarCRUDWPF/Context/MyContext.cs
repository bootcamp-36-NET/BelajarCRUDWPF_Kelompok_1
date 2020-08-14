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

        public MyContext() : base("BelajarCRUDWPF")
        {
        }
    }
}
