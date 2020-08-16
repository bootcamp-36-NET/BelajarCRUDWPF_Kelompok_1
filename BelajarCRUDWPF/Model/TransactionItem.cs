using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelajarCRUDWPF.Model
{
    [Table("transaction_item")]
    class TransactionItem
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }

        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
