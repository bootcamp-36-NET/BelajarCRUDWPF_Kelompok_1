using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelajarCRUDWPF.Model
{
    [Table("tb_m_supplier")]
    class Supplier
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime JoinDate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int IsReset { get; set; }

        public ICollection<Item> Item { get; set; }

    }
}
