using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelajarCRUDWPF.Model
{
    [Table("item")]
    class Item
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        //One to many antara Item ke transactionItem
        public ICollection<TransactionItem> TransactionItems { get; set; }

    }
}