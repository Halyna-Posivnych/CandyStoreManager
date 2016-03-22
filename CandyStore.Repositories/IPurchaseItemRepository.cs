using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CandyStore.Entities;

namespace CandyStore.Repositories
{
    public interface IPurchaseItemRepository
    {
        PurchaseItem AddPurchaseItem(object customerId, int purchaseId, int candyId, decimal amount);
        IEnumerable<PurchaseItem> GetCurrentPurchaseItems(int purchaseId);
        PurchaseItem GetPurchaseItem(int purchaseItemId);
    }
}
