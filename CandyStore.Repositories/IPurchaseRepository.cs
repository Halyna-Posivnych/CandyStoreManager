using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CandyStore.Entities;

namespace CandyStore.Repositories
{
    public interface IPurchaseRepository
    {
        Purchase AddPurchase(object customerId, DateTime time, int userId);
        IEnumerable<Purchase> GetAllPurchases();
        decimal ConfirmPurchase(int purchaseId, object customerId);
    }
}
