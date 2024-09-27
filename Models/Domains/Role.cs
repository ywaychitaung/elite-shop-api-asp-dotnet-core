namespace elite_shop.Models.Domains;

using System.ComponentModel.DataAnnotations;
using elite_shop.Models.BaseModels;

public class Role : ShortIntegerBaseModel
{
    [Required(ErrorMessage = "Role name is required")]
    [MaxLength(50, ErrorMessage = "Role name must be less than 50 characters")]
    public string Name { get; set; }
    
    [MaxLength(255, ErrorMessage = "Role description must be less than 255 characters")]
    public string Description { get; set; }
}
