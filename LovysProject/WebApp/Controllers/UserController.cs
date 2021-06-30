using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Calendar.Models;
using Core.Calendar.Repositories;
using Core.Calendar.Util.Auth;
using Microsoft.AspNetCore.Mvc;
using WebApp.Validators;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post(User user)
        {
            var validator = new UserValidator();
            var validRes = await validator.ValidateAsync(user);
            if (!validRes.IsValid)
            {
                return new BadRequestObjectResult(validRes.ToString(","));
            }

            await _userRepository.Insert(user);
            return Ok("sucess");
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userRepository.Find());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(User user)
        {
            var validator = new UserLoginValidator();
            var validRes = await validator.ValidateAsync(user);
            if (!validRes.IsValid)
            {
                return new BadRequestObjectResult(validRes.ToString(","));
            }

            User userLogged = await _userRepository.FindUser(user.Username, user.Password);

            if (userLogged == null)
            {
                return new BadRequestObjectResult("User not found");
            }

            var token = TokenService.GenerateToken(user);

            return Ok(new {user = userLogged, token});
        }
    }
}