using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Salvo.Models
{
    public static class DbInitializer
    {
        public static void Initialize(SalvoContext context)
        {
            #region Players
            if (!context.Players.Any())
            {
                var players = new Player[]
                {
                    new Player
                    {
                        Email = "j.bauer@ctu.gov",
                        Name = "Jack Bauer",
                        Password = "24"
                    },
                    new Player
                    {
                        Email = "c.obrian@ctu.gov",
                        Name = "Chloe O'Brian",
                        Password = "42"
                    },
                    new Player
                    {
                        Email = "kim_bauer@gmail.com",
                        Name = "Kim Bauer",
                        Password = "kb"
                    },
                    new Player
                    {
                        Email = "t.almeida@ctu.gov",
                        Name = "Tony Almeida",
                        Password = "mole"
                    }
                };
                // recorremos los players
                foreach (Player player in players)
                {
                    context.Players.Add(player);
                }
                // Guardamos los cambios
                context.SaveChanges();
            }
            #endregion
            
            #region Games
            if (!context.Games.Any())
            {
                var games = new Game[]
                {
                    new Game{CreationDate = DateTime.Now},
                    new Game{CreationDate = DateTime.Now.AddHours(1)},
                    new Game{CreationDate = DateTime.Now.AddHours(2)},
                    new Game{CreationDate = DateTime.Now.AddHours(3)},
                };
                // recorremos los Games y los añadimos al contexto
                foreach (Game game in games)
                {
                    context.Games.Add(game);
                }
                // Guardamos los cambios en la base de datos
                context.SaveChanges();
            }
            #endregion

            #region GamePlayer
            if (!context.GamePlayers.Any())
            {
                var GamePlayers = new GamePlayer[]
                {
                    new GamePlayer{ Game = context.Games.Find(1L), JoinDate = DateTime.Now, Player = context.Players.Find(1L)}, //1
                    new GamePlayer{ Game = context.Games.Find(1L), JoinDate = DateTime.Now, Player = context.Players.Find(2L)}, //2
                    new GamePlayer{ Game = context.Games.Find(2L), JoinDate = DateTime.Now, Player = context.Players.Find(1L)}, //3
                    new GamePlayer{ Game = context.Games.Find(2L), JoinDate = DateTime.Now, Player = context.Players.Find(2L)}, //4
                    new GamePlayer{ Game = context.Games.Find(3L), JoinDate = DateTime.Now, Player = context.Players.Find(2L)}, //5
                    new GamePlayer{ Game = context.Games.Find(3L), JoinDate = DateTime.Now, Player = context.Players.Find(4L)}, //6
                    new GamePlayer{ Game = context.Games.Find(4L), JoinDate = DateTime.Now, Player = context.Players.Find(2L)}, //7
                    new GamePlayer{ Game = context.Games.Find(4L), JoinDate = DateTime.Now, Player = context.Players.Find(1L)}, //8
                    new GamePlayer{ Game = context.Games.Find(5L), JoinDate = DateTime.Now, Player = context.Players.Find(4L)}, //9
                    new GamePlayer{ Game = context.Games.Find(5L), JoinDate = DateTime.Now, Player = context.Players.Find(1L)}, //10
                    new GamePlayer{ Game = context.Games.Find(6L), JoinDate = DateTime.Now, Player = context.Players.Find(3L)}, //11
                    new GamePlayer{ Game = context.Games.Find(7L), JoinDate = DateTime.Now, Player = context.Players.Find(4L)}, //12
                    new GamePlayer{ Game = context.Games.Find(8L), JoinDate = DateTime.Now, Player = context.Players.Find(3L)}, //13
                    new GamePlayer{ Game = context.Games.Find(8L), JoinDate = DateTime.Now, Player = context.Players.Find(4L)}, //14
                };
                // recorremos los Games y los añadimos al contexto
                foreach (GamePlayer game in GamePlayers)
                {
                    context.GamePlayers.Add(game);
                }
                // Guardamos los cambios en la base de datos
                context.SaveChanges();
            }
            #endregion
        }
    }
}
