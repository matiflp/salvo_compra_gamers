using Salvo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Repositories
{
    public class PlayerRepository : RepositoryBase<Player>, IPlayerRepository
    {
        public PlayerRepository(SalvoContext repositoryContext) : base(repositoryContext)
        {

        }

        public Player FindByEmail(string email)
        {
            return FindByCondition(player => player.Email == email).FirstOrDefault();
        }

        public Player FindById(long id)
        {
            return FindByCondition(player => player.Id == id)
                .FirstOrDefault();
        }

        public string Save(Player player)
        {
            try
            {
                if (player.Id == 0)
                    Create(player);
                else
                    Update(player);
                SaveChanges();
                return "OK";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            
        }
    }
}
