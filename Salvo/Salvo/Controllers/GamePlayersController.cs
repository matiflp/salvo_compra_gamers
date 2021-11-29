using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Salvo.Models;
using Salvo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Salvo.Controllers
{
    [Route("api/gamePlayers")]
    [ApiController]
    [Authorize("PlayerOnly")]
    public class GamePlayersController : ControllerBase
    {

        private IGamePlayerRepository _repository;
        private IPlayerRepository _playerRepository;

        public GamePlayersController(IGamePlayerRepository repository, IPlayerRepository playerRepository)
        {
            _repository = repository;
            _playerRepository = playerRepository;
        }

        // GET api/<GamePlayersController>/5
        [HttpGet("{id}", Name = "GetGameView")]
        public IActionResult Get(int id)
        {
            try
            {
                // Obtengo el Email del usuario autenticado
                string email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest";

                // Obtención del GP
                var gp = _repository.GetGamePlayerView(id);

                // Verificar si el GP corresponde al mismo Email del usuario autenticado
                if (gp.Player.Email != email)
                    return Forbid();

                var gameView = new GameViewDTO
                {
                    Id = gp.Id,
                    CreationDate = gp.Game.CreationDate,
                    GamePlayers = gp.Game.GamePlayers.Select(gameplayer => new GamePlayerDTO
                    {
                        Id = gameplayer.Id,
                        JoinDate = gameplayer.JoinDate,
                        Player = new PlayerDTO
                        {
                            Id = gameplayer.PlayerId,
                            Email = gameplayer.Player.Email
                        }
                    }).ToList(),
                    Ships = gp.Ships.Select(ships => new ShipDTO
                    {
                        Id = ships.Id,
                        Type = ships.Type,
                        Locations = ships.Locations.Select(locations => new ShipLocationDTO
                        {
                            Id = locations.Id,
                            Location = locations.Location
                        }).ToList()
                    }).ToList(),
                    Salvos = gp.Game.GamePlayers.SelectMany(gameplayersalvo => gameplayersalvo.Salvos.Select(salvo => new SalvoDTO
                    {
                        Id = salvo.Id,
                        Turn = salvo.Turn,
                        Player = new PlayerDTO
                        {
                            Id = gameplayersalvo.Player.Id,
                            Email = gameplayersalvo.Player.Email
                        },
                        Locations = salvo.Locations.Select(salvoLocation => new SalvoLocationDTO
                        {
                            Id = salvoLocation.Id,
                            Location = salvoLocation.Location
                        }).ToList()
                    })).ToList(),
                    Hits = gp.GetHits(),
                    HitsOpponent = gp.GetOpponent()?.GetHits(),
                    Sunks = gp.GetSunks(),
                    SunksOpponent = gp.GetOpponent()?.GetSunks()
                };
                return Ok(gameView);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{id}/ships")]
        public IActionResult Post(long id, [FromBody] List<ShipDTO> ships)
        {
            try
            {
                // Obtengo el usuario autenticado
                string email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest";

                // Obtengo el player de la DB
                Player player = _playerRepository.FindByEmail(email);

                // Obtengo el player con los ships de la DB
                GamePlayer gamePlayer = _repository.FindById(id);

                // Validación
                if (gamePlayer == null)
                    return StatusCode(403, "No existe el juego");

                // Validación
                if (gamePlayer.Player.Id != player.Id)
                    return StatusCode(403, "El usuario no se encuentra en el juego");

                // Validación
                if (gamePlayer.Ships.Count == 5)
                    return StatusCode(403, "Ya se han posicionado los barcos");

                // Si no existen problemas insertamos los barcos
                gamePlayer.Ships = ships.Select(ship => new Ship
                {
                    GamePlayerId = gamePlayer.Id,
                    Type = ship.Type,
                    Locations = ship.Locations.Select(location => new ShipLocation
                    {
                        ShipId = ship.Id,
                        Location = location.Location
                    }).ToList()
                }).ToList();

                // Guardamos en la DB
                _repository.Save(gamePlayer);

                // Retornamos
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{id}/salvos")]
        public IActionResult Post(long id, [FromBody] SalvoDTO salvo)
        {
            try
            {
                // Inicializacion de variables -------------------------------------------------------------------

                // Obtengo el usuario autenticado
                string email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest";

                // Obtengo el player de la DB
                Player player = _playerRepository.FindByEmail(email);

                // Obtengo el Game Player de la DB
                GamePlayer gamePlayer = _repository.FindById(id);

                // Obtengo el oponente
                GamePlayer opGamePlayer = gamePlayer.GetOpponent();
                
                // Turnos de los jugadores
                int playerTurn = 0;
                int opponentTurn = 0;

                // Validaciones ---------------------------------------------------------------------------------

                // Validación - Verificamos que exista una partida
                if (gamePlayer == null)
                    return StatusCode(403, "No existe el juego");

                // Validación - Verificamos que el usuario autenticado este en ese juego
                if (gamePlayer.Player.Id != player.Id)
                    return StatusCode(403, "El usuario no se encuentra en el juego");               
                
                // Validación - Verificamos que exista un oponente para buscar su turno
                if (opGamePlayer != null)
                    opponentTurn = opGamePlayer.Salvos != null ? opGamePlayer.Salvos.Count : 0;
                else
                    return StatusCode(403, "No tienes un oponente");

                // Validación - Verificamos que sea su turno de disparar
                playerTurn = gamePlayer.Salvos != null ? gamePlayer.Salvos.Count + 1 : 1;
                if ((playerTurn - opponentTurn) < -1 || (playerTurn - opponentTurn) > 1)
                    return StatusCode(403, "No se puede adelantar el turno");

                // Validación - Verificamos quien se unio primero para cuando ambos jugadores esten en el mismo turno
                // y quiera disparar la persona que no se unio primero
                if ((playerTurn - opponentTurn) == 1 && gamePlayer.JoinDate > opGamePlayer.JoinDate)
                    return StatusCode(403, "No se puede adelantar el turno!!!!");

                // Validación - Verificamos que el jugador tenga posicionados sus barcos
                if (gamePlayer.Ships.Count == 0)
                    return StatusCode(403, "Debes posicionar tus barcos primero");

                // Validación - Verificamos que se este disparando los 5 salvos al oponente
                // (Esta validación tambien se hace en el front)
                if (salvo.Locations.Count != 5)
                    return StatusCode(403, "Debe indicar todas las posiciones de los salvos");

                // Validación - Verificamos que el oponente tenga posicionados sus barcos
                if (opGamePlayer.Ships.Count == 0)
                    return StatusCode(403, "El oponente todavia no ha posicionado sus barcos");

                // Guardado ----------------------------------------------------------------------------------------

                // Si no existen problemas se insertan los salvos
                gamePlayer.Salvos.Add(new Models.Salvo
                {
                    GamePlayerID = gamePlayer.Id,
                    Turn = playerTurn,
                    Locations = salvo.Locations.Select(salvoLocation => new SalvoLocation
                    {
                        SalvoId = salvoLocation.Id,
                        Location = salvoLocation.Location
                    }).ToList()
                }); 

                // Se almacena en la DB
                _repository.Save(gamePlayer);

                // Retornamos
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
