using Microsoft.AspNetCore.Mvc;
using Salvo.Models;
using Salvo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Salvo.Controllers
{
    [Route("api/players")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private IPlayerRepository _repository;
        public PlayersController(IPlayerRepository repository)
        {
            _repository = repository;
        }

        // POST api/<PlayersController>
        [HttpPost]
        public IActionResult Post([FromBody] PlayerDTO player)
        {
            try
            {
                // Tareas:
                // Agregar una validacion de seguridad respecto de la clave
                // Cantidad de carecteres, mayúsculas, números
                // Verificar estandar de claves?
                // Pueden revisar expresiones regulares.

                // Verificamos que email y password no esten vacios.
                if (String.IsNullOrEmpty(player.Email) || String.IsNullOrEmpty(player.Password))
                    return StatusCode(403, "Datos inválidos");

                // Validamos que la contraseña cumpla con ciertos criterios
                if (!Regex.IsMatch(player.Password, "^(?=\\w*\\d)(?=\\w*[A-Z])(?=\\w*[a-z])\\S{8,16}$"))
                    return StatusCode(403, "La contraseña debe tener al entre 8 y 16 caracteres, al menos un dígito, " +
                        "al menos una minúscula y al menos una mayúscula.");

                // Validamos que el mail ingresado sea valido
                if (!Regex.IsMatch(player.Email, "^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+)*$"))
                    return StatusCode(403, "El email ingresado no es válido");

                // De ser válido obtenemos el mail y verificamos que no este en uso
                Player dbPlayer = _repository.FindByEmail(player.Email);
                if (dbPlayer != null)
                    return StatusCode(403, "Email está en uso");

                Player newPlayer = new Player
                {
                    Email = player.Email,
                    Password = player.Password,
                    Name = player.Name
                };
                
                _repository.Save(newPlayer);
                // Retornamos el nuevo jugador
                return StatusCode(201, newPlayer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
