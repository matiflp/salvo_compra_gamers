using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Salvo.Models;
using Salvo.Models.Auth;
using Salvo.Repositories;
using Salvo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Salvo.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IPlayerRepository _repository;
        private IEmailSender _mailService;
        private IUserService _userService;
        private IConfiguration _configuration;

        public AuthController(IPlayerRepository repository, IEmailSender mailService, IUserService userService, IConfiguration configuration)
        {
            _repository = repository;
            _mailService = mailService;
            _userService = userService;
            _configuration = configuration;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] PlayerDTO player)
        {
            try
            {
                Player user = _repository.FindByEmail(player.Email);
                if (user == null || !String.Equals(user.Password, player.Password))
                    return StatusCode(401, "Email o contraseña incorrectos");

                var claims = new List<Claim>
                {
                    new Claim("Player", user.Email)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                await _mailService.SendEmailAsync(user.Email, 
                                "Nuevo Inicio de Sesion", "<h1>Hola " + user.Name + "! </h1>" +
                                "<p> Se ha detectado un nuevo inicio de sesión en tu cuenta el " + DateTime.Now.Date.ToShortDateString() + " a las " + DateTime.Now.TimeOfDay.ToString().Substring(0,8) + "</p>");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Auth/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //api/auth/confirmemail? userid&token
        [HttpGet("confirmemail")]
        public IActionResult ConfirmEmail(long userId, string token)
        {

            if (string.IsNullOrWhiteSpace(token))
                return NotFound();

            var result = _userService.ConfirmEmail(userId, token);

            if (result.IsSuccess)
            {
                return StatusCode(200, "true");
            }

            return BadRequest();
        }

        // api/auth/forgetpassword
        [HttpGet("forgetpassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return NotFound();

            var result = await _userService.ForgetPasswordAsync(email);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result); // 400
        }

        // api/auth/resetpassword
        [HttpPost("resetpassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.ResetPassword(model);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid");
        }
    }
}
