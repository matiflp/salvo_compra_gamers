using Microsoft.EntityFrameworkCore;
using Salvo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Repositories
{
    public class GamePlayerRepository : RepositoryBase<GamePlayer>, IGamePlayerRepository
    {
        public GamePlayerRepository(SalvoContext repositoryContext) : base(repositoryContext)
        {
        }

        public GamePlayer GetGamePlayerView(int idGamePlayer)
        {
            return FindAll(source => source
                    .Include(gamePlayer => gamePlayer.Ships)
                    .Include(gamePlayer => gamePlayer.Salvos)
                    .Include(gamePlayer => gamePlayer.Player)
                    .Include(gamePlayer => gamePlayer.Game)
                        .ThenInclude(game => game.GamePlayers)
                            .ThenInclude(gp => gp.Player)
                                .ThenInclude(player => player.Scores)
                    .Include(gamePlayer => gamePlayer.Game)
                        .ThenInclude(game => game.GamePlayers)
                            .ThenInclude(gp => gp.Salvos)
                                .ThenInclude(salvo => salvo.Locations)
                   .Include(gamePlayer => gamePlayer.Game)
                        .ThenInclude(game => game.GamePlayers)
                            .ThenInclude(gp => gp.Ships)
                                .ThenInclude(ship => ship.Locations))
                                
                .Where(gamePlayer => gamePlayer.Id == idGamePlayer)
                .OrderBy(game => game.JoinDate)
                .FirstOrDefault();
        }

        public void Save(GamePlayer gamePlayer)
        {
            if (gamePlayer.Id == 0)
                Create(gamePlayer);
            else
                Update(gamePlayer);
            SaveChanges();
        }

        public GamePlayer FindById(long Id)
        {
            return FindByCondition(gamePlayer => gamePlayer.Id == Id)
                .Include(gamePlayer => gamePlayer.Player)
                .Include(gamaPlayer => gamaPlayer.Ships)
                    .ThenInclude(ship => ship.Locations)
                .Include(gamePlayer => gamePlayer.Salvos)
                .Include(gamePlayer => gamePlayer.Game)
                    .ThenInclude(game => game.GamePlayers)
                        .ThenInclude(gp => gp.Salvos)
                            .ThenInclude(salvo => salvo.Locations)
                .Include(gamePlayer => gamePlayer.Game)
                    .ThenInclude(game => game.GamePlayers)
                        .ThenInclude(gp => gp.Ships)
                            .ThenInclude(ship => ship.Locations)
                .FirstOrDefault();       
        }
    }
}
