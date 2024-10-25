using elite_shop.Exceptions;

namespace elite_shop.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using elite_shop.Models.DTOs.Requests;
using elite_shop.Services.Interfaces;
using System;

[ApiController]
[Route("api/auth")]
public class AuthApiController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthApiController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("customers/register")]
    public async Task<IActionResult> RegisterCustomer([FromBody] UserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Delegate registration to UserService
            var token = await _userService.RegisterCustomerAsync(userDto);
            return Ok(new { Token = token });
        }
        catch (EmailAlreadyInUseException ex)
        {
            // Return a 409 Conflict for email uniqueness violations
            return Conflict(new { Message = ex.Message });
        }
        catch (Exception)
        {
            // Catch any other exceptions and return a 500 Internal Server Error
            return StatusCode(500, new { Message = "An error occurred while processing your request." });
        }
    }
}
