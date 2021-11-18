﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class GamesController : ControllerBase
    {
        private IGameRepository _repository;
        private IPlayerRepository _playerRepository;
        private IGamePlayerRepository _gamePlayerRepository;

        public GamesController(IGameRepository repository, 
            IPlayerRepository playerRepository, 
            IGamePlayerRepository gamePlayerRepository)
        {
            _repository = repository;
            _playerRepository = playerRepository;
            _gamePlayerRepository = gamePlayerRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            try
            {
                GameListDTO gameList = new GameListDTO
                {
                    Email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest",
                    Games = _repository.GetAllGamesWithPlayers()
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
                            Point = gameplayer.GetScore() != null ? (double?)gameplayer.GetScore().Point : null
                        }).ToList()
                    }).ToList()
                };
                return Ok(gameList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post()
        {
            try
            {
                string email = User.FindFirst("Player") != null ? User.FindFirst("Player").Value : "Guest";

                // Vamos a buscar al jugador autenticado
                Player player = _playerRepository.FindByEmail(email);
                DateTime fechaActual = DateTime.Now;
                GamePlayer gamePlayer = new GamePlayer
                {
                    Game = new Game
                    {
                        CreationDate = fechaActual
                    },
                    PlayerId = player.Id,
                    JoinDate = fechaActual
                };
                // Guardar el gamePlayer
                _gamePlayerRepository.Save(gamePlayer);

                return StatusCode(201, gamePlayer.Id);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
