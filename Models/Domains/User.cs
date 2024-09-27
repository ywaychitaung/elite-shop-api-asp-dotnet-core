namespace elite_shop.Models.Domains;

using System.ComponentModel.DataAnnotations;
using elite_shop.Models.BaseModels;
public class User : UuidBaseModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    [MaxLength(100, ErrorMessage = "Email must be less than 100 characters")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    [MaxLength(50, ErrorMessage = "Password must be less than 50 characters")]
    public string Password { get; set; }
    
    public string SaltKey { get; set; }
    
    public bool IsActive { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public DateTime LastLoginAt { get; set; }
    
    // Relationships
    public Role role { get; set; }
}
