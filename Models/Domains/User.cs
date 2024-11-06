using System.ComponentModel.DataAnnotations.Schema;

namespace elite_shop.Models.Domains;

using System.ComponentModel.DataAnnotations;
using elite_shop.Models.BaseModels;
public class User : UuidBaseModel
{
    [Required(ErrorMessage = "Email is required")]
    public byte[] Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public byte[] Password { get; set; }
    
    public byte[] SaltKey { get; set; }
    
    public bool IsActive { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public DateTime LastLoginAt { get; set; }
    
    // Foreign Key for Role
    [ForeignKey("Role")]
    public short RoleId { get; set; }

    // Navigation property for Role
    public Role Role { get; set; }
}
