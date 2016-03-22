using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandyStore.Entities
{
    public class Candy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public decimal Price { get; set; }
        public decimal SupplyQty { get; set; }
        public decimal Promotion { get; set; }
        public bool Deleted { get; set; }
    }
}
