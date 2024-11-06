using elite_shop.Services.UtilityServices.Implementations;
using elite_shop.Services.UtilityServices.Interfaces;

namespace elite_shop.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Exceptions;
using Models.DTOs.Requests;
using Services.ModelServices.Interfaces;
using System;

[ApiController]
[Route("api/auth")]
public class AuthApiController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILoginRateLimitService _loginRateLimitService;

    public AuthApiController(IUserService userService, ILoginRateLimitService loginRateLimitService)
    {
        _userService = userService;
        _loginRateLimitService = loginRateLimitService;
    }

    [HttpPost("customers/register")]
    public async Task<IActionResult> RegisterCustomer([FromBody] CustomerRegisterRequestDto customerRegisterRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Delegate registration to UserService
            var token = await _userService.RegisterCustomerAsync(customerRegisterRequestDto);
            return Ok(new { Token = token });
        }
        catch (EmailAlreadyInUseException ex)
        {
            // Return a 409 Conflict for email uniqueness violations
            return Conflict(new { ex.Message });
        }
        catch (Exception)
        {
            // Catch any other exceptions and return a 500 Internal Server Error
            return StatusCode(500, new { Message = "An error occurred while processing your request." });
        }
    }
    
    [HttpPost("customers/login")]
    public async Task<IActionResult> LoginCustomer([FromBody] CustomerLoginRequestDto customerLoginRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Check if the user is locked out due to too many failed attempts
            if (await _loginRateLimitService.IsLockedOutAsync(customerLoginRequestDto.Email))
            {
                return BadRequest(new { Message = "Too many failed attempts. Please try again later." });
            }

            // Attempt to log in the user
            var token = await _userService.LoginCustomerAsync(customerLoginRequestDto);

            // Reset the failed attempt count on successful login
            await _loginRateLimitService.ResetAttemptsAsync(customerLoginRequestDto.Email);

            return Ok(new { Token = token });
        }
        catch (InvalidCredentialsException ex)
        {
            // Record the failed attempt if the login is unsuccessful
            await _loginRateLimitService.RecordFailedAttemptAsync(customerLoginRequestDto.Email);
            return Unauthorized(new { Message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { Message = "An error occurred while processing your request." });
        }
    }
}
