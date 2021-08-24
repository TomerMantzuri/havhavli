using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace havhavli.Models
{
    public class SupplierProducts
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product product { get; set; }
        public int InStock { get; set; }
        public int SupplierId { get; set; }
        public Supplier supplier { get; set; }
    }
}
