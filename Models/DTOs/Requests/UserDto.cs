using System.ComponentModel.DataAnnotations;

namespace elite_shop.Models.DTOs.Requests;

public class UserDto
{
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}
