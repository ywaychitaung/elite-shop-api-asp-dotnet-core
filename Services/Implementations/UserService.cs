using elite_shop.Exceptions;
using Microsoft.Data.SqlClient;

namespace elite_shop.Services.Implementations;

using AutoMapper;
using Microsoft.EntityFrameworkCore;

using elite_shop.Helpers;
using elite_shop.Models.Domains;
using elite_shop.Models.DTOs.Requests;
using elite_shop.Repositories.Interfaces;
using elite_shop.Services.Interfaces;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtHelper _jwtHelper;
    private readonly EncryptionHelper _encryptionHelper;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, JwtHelper jwtHelper, EncryptionHelper encryptionHelper, IMapper mapper)
    {
        _userRepository = userRepository;
        _jwtHelper = jwtHelper;
        _encryptionHelper = encryptionHelper;
        _mapper = mapper;
    }

    public async Task<string> RegisterCustomerAsync(CustomerRegisterRequestDto customerRegisterRequestDto)
    {
        // Map UserDto to User
        var user = _mapper.Map<User>(customerRegisterRequestDto);

        // Encrypt the email
        user.Email = _encryptionHelper.Encrypt(customerRegisterRequestDto.Email);

        // Hash the password and generate a salt
        string salt;
        user.Password = HashHelper.HashPassword(customerRegisterRequestDto.Password, out salt);
        user.SaltKey = salt;

        try
        {
            // Save the user to the database using the repository
            await _userRepository.AddUserAsync(user);
        }
        catch (DbUpdateException ex)
        {
            // Check if the inner exception is an SqlException
            if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601) // 2601 is for unique constraint violation
            {
                // Email already exists, throw the custom exception
                throw new EmailAlreadyInUseException();
            }
            throw; // Re-throw the original exception if it's not related to email uniqueness
        }

        // Generate JWT token
        return _jwtHelper.GenerateToken(user);
    }
    
    public async Task<string> LoginCustomerAsync(CustomerLoginRequestDto customerLoginRequestDto)
    {
        // Encrypt the email to match the stored, encrypted version
        var encryptedEmail = _encryptionHelper.Encrypt(customerLoginRequestDto.Email);

        // Fetch the user by the encrypted email
        var user = await _userRepository.GetUserByEmailAsync(encryptedEmail);

        // Check if the user exists
        if (user == null)
        {
            // User not found, throw an exception
            throw new InvalidCredentialsException("Invalid email or password.");
        }

        // Verify the password
        if (!HashHelper.VerifyPassword(customerLoginRequestDto.Password, user.Password, user.SaltKey))
        {
            // Password is incorrect, throw an exception
            throw new InvalidCredentialsException("Invalid email or password.");
        }

        // Generate JWT token
        return _jwtHelper.GenerateToken(user);
    }
}
