using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CandyStore.Entities;

namespace CandyStore.Repositories
{
    public interface IUserRepository
    {
        User GetUserByLogin(string login, string password);
    }
}
