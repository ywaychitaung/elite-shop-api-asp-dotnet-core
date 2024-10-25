namespace elite_shop.Services.Interfaces;

using elite_shop.Models.DTOs.Requests;

public interface IUserService
{
    Task<string> RegisterCustomerAsync(CustomerRegisterRequestDto customerRegisterRequestDto);
    Task<string> LoginCustomerAsync(CustomerLoginRequestDto customerLoginRequestDto);
}
