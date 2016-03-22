using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandyStore.Entities
{
    public class PurchaseItem
    {
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public int CandyId { get; set; }
        public decimal Amount { get; set; }
        public decimal Cost { get; set; }
    }
}
