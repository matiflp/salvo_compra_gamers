using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Salvo.Models;
using Salvo.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Salvo.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private IGameRepository _repository;

        public GamesController(IGameRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var games = _repository.GetAllGamesWithPlayers()
                    .Select(game => new GameDTO
                    {
                        Id = game.Id,
                        CreationDate = game.CreationDate,
                        GamePlayers = game.GamePlayers.Select(gameplayer => new GamePlayerDTO
                        {
                            Id = gameplayer.Id,
                            JoinDate = gameplayer.JoinDate,
                            Player = new PlayerDTO
                            {
                                Id = gameplayer.PlayerId,
                                Email = gameplayer.Player.Email
                            },
                            Point = gameplayer.GetScore() != null ? (double?) gameplayer.GetScore().Point : null
                        }).ToList()
                    }).ToList();
                return Ok(games);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
