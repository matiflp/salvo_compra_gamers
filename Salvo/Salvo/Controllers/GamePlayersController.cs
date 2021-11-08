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
    public class GamePlayersController : ControllerBase
    {

        private IGamePlayerRepository _repository;

        public GamePlayersController(IGamePlayerRepository repository)
        {
            _repository = repository;
        }

        // GET api/<GamePlayersController>/5
        [HttpGet("{id}", Name = "GetGameView")]
        public IActionResult Get(int id)
        {
            try
            {
                var gp = _repository.GetGamePlayerView(id);
                var gpDTO = new GameViewDTO
                {
                    Id = gp.Game.Id,
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
                    }).ToList()
                };
                return Ok(gpDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

     
    }
}
