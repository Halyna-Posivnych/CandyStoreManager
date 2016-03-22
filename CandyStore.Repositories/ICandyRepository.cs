using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CandyStore.Entities;

namespace CandyStore.Repositories
{
    public interface ICandyRepository
    {
        IEnumerable<Candy> GetAvailableCandies();
        void ChangePromotion(int candyId, decimal newPromotion);
        void DeleteCandy(int candyId);
        void ReplenishCandySupply(int candyId, decimal supplyQty);
        Candy AddNewCandy(string name, string manufacturee, decimal price, decimal supplyQty);
        Candy GetCandy(int candyId);
        bool CandyIsAvailable(int candyId, decimal amount);
    }
}
