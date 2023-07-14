using WarehouseAPI.Models.Dto;
using WarehouseAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.Json.Serialization;
using WarehouseAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace WarehouseAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _accountService;

        public AuthController(IAuthenticationService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("session")]
        public ActionResult Get()
        {
            var token = HttpContext.Request.Headers["Authorization"];
            var result = _accountService.GetLoggedUser(token);
            string json = JsonSerializer.Serialize(result);
            return Ok(json);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterDataDto dto)
        {
            string result = _accountService.RegisterUser(dto);

            if (!result.Equals("ok")) return StatusCode(StatusCodes.Status406NotAcceptable, JsonSerializer.Serialize(new { message = result }));

            var response = new { message = "Registration successful!" };

            return Ok(JsonSerializer.Serialize(response));
        }

        [HttpPost("login")]
        public ActionResult<LoggedUserRecordDto> Login([FromBody] LoginDto dto)
        {
            return _accountService.GenerateJwtAndGetUser(dto);
		}
    }
}
