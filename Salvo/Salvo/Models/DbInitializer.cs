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

            #region Ships
            if (!context.Ships.Any())
            {
                GamePlayer gamePlayer1 = context.GamePlayers.Find(1L);
                GamePlayer gamePlayer2 = context.GamePlayers.Find(2L);
                GamePlayer gamePlayer3 = context.GamePlayers.Find(3L);
                GamePlayer gamePlayer4 = context.GamePlayers.Find(4L);
                GamePlayer gamePlayer5 = context.GamePlayers.Find(5L);
                GamePlayer gamePlayer6 = context.GamePlayers.Find(6L);
                GamePlayer gamePlayer7 = context.GamePlayers.Find(7L);
                GamePlayer gamePlayer8 = context.GamePlayers.Find(8L);
                GamePlayer gamePlayer9 = context.GamePlayers.Find(9L);
                GamePlayer gamePlayer10 = context.GamePlayers.Find(10L);
                GamePlayer gamePlayer11 = context.GamePlayers.Find(11L);
                GamePlayer gamePlayer12 = context.GamePlayers.Find(12L);
                GamePlayer gamePlayer13 = context.GamePlayers.Find(13L);

                var ships = new Ship[]
                {
                    //esta es solo la primera linea de los datos del pdf
                    new Ship
                    {
                        Type = "Destroyer",
                        GamePlayer = gamePlayer1,
                        Locations = new ShipLocation[]
                        {
                            new ShipLocation{Location = "H2"},
                            new ShipLocation{Location = "H3"},
                            new ShipLocation{Location = "H4"}
                        }
                    },
                    new Ship
                    {
                        Type = "Submarine", 
                        GamePlayer = gamePlayer1, 
                        Locations = new ShipLocation[]
                        {
                            new ShipLocation { Location = "E1" },
                            new ShipLocation { Location = "F1" },
                            new ShipLocation { Location = "G1" }
                        }
                    },
                    new Ship
                    {
                        Type = "PatroalBoat", 
                        GamePlayer = gamePlayer1, 
                        Locations = new ShipLocation[] 
                        {
                            new ShipLocation { Location = "B4" },
                            new ShipLocation { Location = "B5" }
                        }
                    },

                    //obrian gp2
                    new Ship
                    {
                        Type = "Destroyer", 
                        GamePlayer = gamePlayer2, 
                        Locations = new ShipLocation[] 
                        {
                            new ShipLocation { Location = "B5" },
                            new ShipLocation { Location = "C5" },
                            new ShipLocation { Location = "D5" }
                        }
                    },
                    new Ship
                    {
                        Type = "PatroalBoat", 
                        GamePlayer = gamePlayer2, 
                        Locations = new ShipLocation[] 
                        {
                            new ShipLocation { Location = "F1" },
                            new ShipLocation { Location = "F2" }
                        }
                    },

                    //jbauer gp3
                    new Ship
                    {
                        Type = "Destroyer", 
                        GamePlayer = gamePlayer3, 
                        Locations = new ShipLocation[]
                        {
                            new ShipLocation { Location = "B5" },
                            new ShipLocation { Location = "C5" },
                            new ShipLocation { Location = "D5" }
                        }
                    },
                    new Ship
                    {
                        Type = "PatroalBoat", 
                        GamePlayer = gamePlayer3, 
                        Locations = new ShipLocation[] 
                        {
                            new ShipLocation { Location = "C6" },
                            new ShipLocation { Location = "C7" }
                        }
                    },

                    //obrian gp4
                    new Ship
                    {
                        Type = "Submarine", 
                        GamePlayer = gamePlayer4, 
                        Locations = new ShipLocation[] 
                        {
                            new ShipLocation { Location = "A2" },
                            new ShipLocation { Location = "A3" },
                            new ShipLocation { Location = "A4" }
                        }
                    },
                    new Ship
                    {
                        Type = "PatroalBoat", 
                        GamePlayer = gamePlayer4, 
                        Locations = new ShipLocation[] 
                        {
                            new ShipLocation { Location = "G6" },
                            new ShipLocation { Location = "H6" }
                        }
                    },

                    //obrian gp5
                    new Ship
                    {
                        Type = "Destroyer", 
                        GamePlayer = gamePlayer5,
                        Locations = new ShipLocation[]
                        {
                            new ShipLocation { Location = "B5" },
                            new ShipLocation { Location = "C5" },
                            new ShipLocation { Location = "D5" }
                        }
                    },
                    new Ship
                    {
                        Type = "PatroalBoat",
                        GamePlayer = gamePlayer5, 
                        Locations = new ShipLocation[]
                        {
                            new ShipLocation { Location = "C6" },
                            new ShipLocation { Location = "C7" }
                        }
                    },

                    //talmeida gp6
                    new Ship
                    {
                        Type = "Submarine", 
                        GamePlayer = gamePlayer6, 
                        Locations = new ShipLocation[] 
                        {
                            new ShipLocation { Location = "A2" },
                            new ShipLocation { Location = "A3" },
                            new ShipLocation { Location = "A4" }
                        }
                    },
                    new Ship
                    {
                        Type = "PatroalBoat",
                        GamePlayer = gamePlayer6, 
                        Locations = new ShipLocation[] 
                        {
                            new ShipLocation { Location = "G6" },
                            new ShipLocation { Location = "H6" }
                        }
                    },

                    //obrian gp7
                    new Ship
                    {
                        Type = "Destroyer", 
                        GamePlayer = gamePlayer7, 
                        Locations = new ShipLocation[] 
                        {
                            new ShipLocation { Location = "B5" },
                            new ShipLocation { Location = "C5" },
                            new ShipLocation { Location = "D5" }
                        }
                    },
                    new Ship
                    {
                        Type = "PatroalBoat", 
                        GamePlayer = gamePlayer7, 
                        Locations = new ShipLocation[] 
                        {
                            new ShipLocation { Location = "C6" },
                            new ShipLocation { Location = "C7" }
                        }
                    },

                    //jbauer gp8
                    new Ship
                    {
                        Type = "Submarine", 
                        GamePlayer = gamePlayer8,
                        Locations = new ShipLocation[] 
                        {
                            new ShipLocation { Location = "A2" },
                            new ShipLocation { Location = "A3" },
                            new ShipLocation { Location = "A4" }
                        }
                    },
                    new Ship
                    {
                        Type = "PatroalBoat", 
                        GamePlayer = gamePlayer8, 
                        Locations = new ShipLocation[] 
                        {
                            new ShipLocation { Location = "G6" },
                            new ShipLocation { Location = "H6" }
                        }
                    },

                    //talmeida gp9
                    new Ship
                    {
                        Type = "Destroyer",
                        GamePlayer = gamePlayer9, 
                        Locations = new ShipLocation[] 
                        {
                            new ShipLocation { Location = "B5" },
                            new ShipLocation { Location = "C5" },
                            new ShipLocation { Location = "D5" }
                        }
                    },
                    new Ship
                    {
                        Type = "PatroalBoat",
                        GamePlayer = gamePlayer9,
                        Locations = new ShipLocation[] 
                        {
                            new ShipLocation { Location = "C6" },
                            new ShipLocation { Location = "C7" }
                        }
                    },

                    //jbauer gp10
                    new Ship
                    {
                        Type = "Submarine", 
                        GamePlayer = gamePlayer10, 
                        Locations = new ShipLocation[] 
                        {
                            new ShipLocation { Location = "A2" },
                            new ShipLocation { Location = "A3" },
                            new ShipLocation { Location = "A4" }
                        }
                    },
                    new Ship
                    {
                        Type = "PatroalBoat",
                        GamePlayer = gamePlayer10, 
                        Locations = new ShipLocation[]
                        {
                            new ShipLocation { Location = "G6" },
                            new ShipLocation { Location = "H6" }
                        }
                    },

                    //kbauer gp11
                    new Ship
                    {
                        Type = "Destroyer", 
                        GamePlayer = gamePlayer11, 
                        Locations = new ShipLocation[] 
                        {
                            new ShipLocation { Location = "B5" },
                            new ShipLocation { Location = "C5" },
                            new ShipLocation { Location = "D5" }
                        }
                    },
                    new Ship
                    {
                        Type = "PatroalBoat",
                        GamePlayer = gamePlayer11,
                        Locations = new ShipLocation[] 
                        {
                            new ShipLocation { Location = "C6" },
                            new ShipLocation { Location = "C7" }
                        }
                    },

                    //kbauer gp12
                    new Ship
                    {
                        Type = "Destroyer", 
                        GamePlayer = gamePlayer12, 
                        Locations = new ShipLocation[]
                        {
                            new ShipLocation { Location = "B5" },
                            new ShipLocation { Location = "C5" },
                            new ShipLocation { Location = "D5" }
                        }
                    },
                    new Ship
                    {
                        Type = "PatroalBoat", 
                        GamePlayer = gamePlayer12, 
                        Locations = new ShipLocation[]
                        {
                            new ShipLocation { Location = "C6" },
                            new ShipLocation { Location = "C7" }
                        }
                    },

                    //talmeida gp13
                    new Ship
                    {
                        Type = "Submarine",
                        GamePlayer = gamePlayer13,
                        Locations = new ShipLocation[] 
                        {
                            new ShipLocation { Location = "A2" },
                            new ShipLocation { Location = "A3" },
                            new ShipLocation { Location = "A4" }
                        }
                    },
                    new Ship
                    {
                        Type = "PatroalBoat",
                        GamePlayer = gamePlayer13,
                        Locations = new ShipLocation[] 
                        {
                            new ShipLocation { Location = "G6" },
                            new ShipLocation { Location = "H6" }
                        }
                    },
                };

                foreach (Ship ship in ships)
                {
                    context.Ships.Add(ship);
                }

                context.SaveChanges();
            }
            #endregion

            #region Salvos
            if (!context.Salvos.Any())
            {
                GamePlayer gamePlayer1 = context.GamePlayers.Find(1L);
                GamePlayer gamePlayer2 = context.GamePlayers.Find(2L);
                GamePlayer gamePlayer3 = context.GamePlayers.Find(3L);
                GamePlayer gamePlayer4 = context.GamePlayers.Find(4L);
                GamePlayer gamePlayer5 = context.GamePlayers.Find(5L);
                GamePlayer gamePlayer6 = context.GamePlayers.Find(6L);
                GamePlayer gamePlayer7 = context.GamePlayers.Find(7L);
                GamePlayer gamePlayer8 = context.GamePlayers.Find(8L);
                GamePlayer gamePlayer9 = context.GamePlayers.Find(9L);
                GamePlayer gamePlayer10 = context.GamePlayers.Find(10L);

                var salvos = new Salvo[]
                {
                    //Game = 1 ; Turn = 1
                    new Salvo
                    {
                        Turn = 1,
                        Gameplayer = gamePlayer1,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "B5"},
                            new SalvoLocation{Location = "C5"},
                            new SalvoLocation{Location = "F1"}
                        }
                    },
                    new Salvo
                    {
                        Turn = 1,
                        Gameplayer = gamePlayer2,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "B4"},
                            new SalvoLocation{Location = "B5"},
                            new SalvoLocation{Location = "B6"}
                        }
                    },
                    //Game = 1 ; Turn = 2
                    new Salvo
                    {
                        Turn = 2,
                        Gameplayer = gamePlayer1,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "F2"},
                            new SalvoLocation{Location = "D5"},
                        }
                    },
                    new Salvo
                    {
                        Turn = 2,
                        Gameplayer = gamePlayer2,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "E1"},
                            new SalvoLocation{Location = "H3"},
                            new SalvoLocation{Location = "A2"}
                        }
                    },
                    //Game = 2 ; Turn = 1
                    new Salvo
                    {
                        Turn = 1,
                        Gameplayer = gamePlayer3,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "A2"},
                            new SalvoLocation{Location = "A4"},
                            new SalvoLocation{Location = "G6"}
                        }
                    },
                    new Salvo
                    {
                        Turn = 1,
                        Gameplayer = gamePlayer4,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "B5"},
                            new SalvoLocation{Location = "D5"},
                            new SalvoLocation{Location = "C7"}
                        }
                    },
                    //Game = 2 ; Turn = 2
                    new Salvo
                    {
                        Turn = 2,
                        Gameplayer = gamePlayer3,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "A3"},
                            new SalvoLocation{Location = "H6"}
                        }
                    },
                    new Salvo
                    {
                        Turn = 2,
                        Gameplayer = gamePlayer4,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "C5"},
                            new SalvoLocation{Location = "C6"}
                        }
                    },
                    //Game = 3 ; Turn = 1
                    new Salvo
                    {
                        Turn = 1,
                        Gameplayer = gamePlayer5,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "G6"},
                            new SalvoLocation{Location = "H6"},
                            new SalvoLocation{Location = "A4"}
                        }
                    },
                    new Salvo
                    {
                        Turn = 1,
                        Gameplayer = gamePlayer6,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "H1"},
                            new SalvoLocation{Location = "H2"},
                            new SalvoLocation{Location = "H3"}
                        }
                    },
                    //Game = 3 ; Turn = 2
                    new Salvo
                    {
                        Turn = 2,
                        Gameplayer = gamePlayer5,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "A2"},
                            new SalvoLocation{Location = "A3"},
                            new SalvoLocation{Location = "D8"}
                        }
                    },
                    new Salvo
                    {
                        Turn = 2,
                        Gameplayer = gamePlayer6,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "E1"},
                            new SalvoLocation{Location = "F2"},
                            new SalvoLocation{Location = "G3"},
                        }
                    },
                    //Game = 4 ; Turn = 1
                    new Salvo
                    {
                        Turn = 1,
                        Gameplayer = gamePlayer7,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "A3"},
                            new SalvoLocation{Location = "A4"},
                            new SalvoLocation{Location = "F7"}
                        }
                    },
                    new Salvo
                    {
                        Turn = 1,
                        Gameplayer = gamePlayer8,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "B5"},
                            new SalvoLocation{Location = "C6"},
                            new SalvoLocation{Location = "H1"},
                        }
                    },
                    //Game = 4 ; Turn = 2
                    new Salvo
                    {
                        Turn = 2,
                        Gameplayer = gamePlayer7,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "A2"},
                            new SalvoLocation{Location = "G6"},
                            new SalvoLocation{Location = "H6"}
                        }
                    },
                    new Salvo
                    {
                        Turn = 2,
                        Gameplayer = gamePlayer8,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "C5"},
                            new SalvoLocation{Location = "C7"},
                            new SalvoLocation{Location = "D5"},
                        }
                    },
                    //Game = 5 ; Turn = 1
                    new Salvo
                    {
                        Turn = 2,
                        Gameplayer = gamePlayer9,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "A1"},
                            new SalvoLocation{Location = "A2"},
                            new SalvoLocation{Location = "A3"}
                        }
                    },
                    new Salvo
                    {
                        Turn = 2,
                        Gameplayer = gamePlayer10,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "B5"},
                            new SalvoLocation{Location = "B6"},
                            new SalvoLocation{Location = "C7"},
                        }
                    },
                    //Game = 5 ; Turn = 2
                    new Salvo
                    {
                        Turn = 2,
                        Gameplayer = gamePlayer9,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "G6"},
                            new SalvoLocation{Location = "G7"},
                            new SalvoLocation{Location = "G8"}
                        }
                    },
                    new Salvo
                    {
                        Turn = 2,
                        Gameplayer = gamePlayer10,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "C6"},
                            new SalvoLocation{Location = "D6"},
                            new SalvoLocation{Location = "E6"},
                        }
                    },
                    //Game = 5 ; Turn = 3
                    new Salvo
                    {
                        Turn = 3,
                        Gameplayer = gamePlayer10,
                        Locations = new SalvoLocation[]
                        {
                            new SalvoLocation{Location = "H1"},
                            new SalvoLocation{Location = "H8"},
                        }
                    }
                };

                foreach (Salvo salvo in salvos)
                {
                    context.Salvos.Add(salvo);
                }

                context.SaveChanges();
            }
            #endregion

            #region Scores
            if (!context.Scores.Any()) 
            {
                var Scores = new Score[]
                {
                    new Score{ Game = context.Games.Find(1L), FinishDate = DateTime.Now, Player = context.Players.Find(1L), Point = 1.0}, //1
                    new Score{ Game = context.Games.Find(1L), FinishDate = DateTime.Now, Player = context.Players.Find(2L), Point = 0.0}, //2
                    new Score{ Game = context.Games.Find(2L), FinishDate = DateTime.Now, Player = context.Players.Find(1L), Point = 0.5}, //3
                    new Score{ Game = context.Games.Find(2L), FinishDate = DateTime.Now, Player = context.Players.Find(2L), Point = 0.5}, //4
                    new Score{ Game = context.Games.Find(3L), FinishDate = DateTime.Now, Player = context.Players.Find(2L), Point = 1.0}, //5
                    new Score{ Game = context.Games.Find(3L), FinishDate = DateTime.Now, Player = context.Players.Find(4L), Point = 0.0}, //6
                    new Score{ Game = context.Games.Find(4L), FinishDate = DateTime.Now, Player = context.Players.Find(2L), Point = 0.5}, //7
                    new Score{ Game = context.Games.Find(4L), FinishDate = DateTime.Now, Player = context.Players.Find(1L), Point = 0.5}, //8
                    new Score{ Game = context.Games.Find(5L), FinishDate = DateTime.Now, Player = context.Players.Find(4L)}, //9
                    new Score{ Game = context.Games.Find(5L), FinishDate = DateTime.Now, Player = context.Players.Find(1L)}, //10
                    new Score{ Game = context.Games.Find(6L), FinishDate = DateTime.Now, Player = context.Players.Find(3L)}, //11
                    new Score{ Game = context.Games.Find(7L), FinishDate = DateTime.Now, Player = context.Players.Find(4L)}, //12
                    new Score{ Game = context.Games.Find(8L), FinishDate = DateTime.Now, Player = context.Players.Find(3L)}, //13
                    new Score{ Game = context.Games.Find(8L), FinishDate = DateTime.Now, Player = context.Players.Find(4L)}, //14
                };
                // recorremos los Scores y los añadimos al contexto
                foreach (Score score in Scores)
                {
                    context.Scores.Add(score);
                }
                // Guardamos los cambios en la base de datos
                context.SaveChanges();
            }
            #endregion
        }
    }
}
