using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandyStore.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal Cost { get; set; }
        public DateTime PurchaseTime { get; set; }
        public int UserId { get; set; }
    }
}
