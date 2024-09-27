namespace elite_shop.Models.BaseModels;

using System.ComponentModel.DataAnnotations;

public abstract class UuidBaseModel
{
    [Key]
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Auto-generate the Id and timestamps when a new instance is created
    protected UuidBaseModel()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }
}
