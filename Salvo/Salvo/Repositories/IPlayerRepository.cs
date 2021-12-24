using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Salvo.Models;

namespace Salvo.Repositories
{
    public interface IPlayerRepository
    {
        public Player FindByEmail(string email);
        public Player FindById(long id);
        public string Save(Player player);
    }
}
