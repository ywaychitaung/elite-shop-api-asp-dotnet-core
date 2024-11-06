namespace elite_shop.Data;

using Microsoft.EntityFrameworkCore;
using Models.BaseModels;
using Models.Domains;
using Seeders;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Convert all table names to snake_case
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Get the table name for the entity
            var tableName = entityType.GetTableName();
            
            // If the table name is not null or empty, convert it to snake_case
            if (!string.IsNullOrEmpty(tableName))
            {
                // Set the table name to snake_case
                modelBuilder.Entity(entityType.ClrType).ToTable(ConvertToSnakeCase(tableName));
            }
        }
        
        // UuidBaseModel
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                     .Where(t => typeof(UuidBaseModel).IsAssignableFrom(t.ClrType)))
        {
            // Set the CreatedAt column to datetime2(7)
            modelBuilder.Entity(entityType.ClrType)
                .Property("CreatedAt")
                .HasColumnType("datetime2(7)");

            // Set the UpdatedAt column to datetime2(7)
            modelBuilder.Entity(entityType.ClrType)
                .Property("UpdatedAt")
                .HasColumnType("datetime2(7)");
        }
        
        // ShortIntegerBaseModel
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                     .Where(t => typeof(ShortIntegerBaseModel).IsAssignableFrom(t.ClrType)))
        {
            // Set the CreatedAt column to datetime2(7)
            modelBuilder.Entity(entityType.ClrType)
                .Property("CreatedAt")
                .HasColumnType("datetime2(7)");

            // Set the UpdatedAt column to datetime2(7)
            modelBuilder.Entity(entityType.ClrType)
                .Property("UpdatedAt")
                .HasColumnType("datetime2(7)");
        }
        
        // Ensure Email is unique
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Call the base OnModelCreating method
        base.OnModelCreating(modelBuilder);
        
        // Call ApplicationDbSeeder to seed all data
        ApplicationDbSeeder.SeedAll(modelBuilder);
    }

    // Automatically update the timestamps when saving changes    
    public override int SaveChanges()
    {
        // Get all entries that are UuidBaseModel or ShortIntegerBaseModel and are added or modified
        var entries = ChangeTracker
            .Entries()
            .Where(e => (e.Entity is UuidBaseModel || e.Entity is ShortIntegerBaseModel) &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        // Loop through each entry
        foreach (var entry in entries)
        {
            // If the entity is UuidBaseModel, update the timestamps
            if (entry.Entity is UuidBaseModel uuidEntity)
            {
                // Update the UpdatedAt timestamp
                uuidEntity.UpdatedAt = DateTime.UtcNow;

                // If the entity is being added, update the CreatedAt timestamp
                if (entry.State == EntityState.Added)
                {
                    // Set the CreatedAt timestamp to the current time
                    uuidEntity.CreatedAt = DateTime.UtcNow;
                }
            }

            // If the entity is ShortIntegerBaseModel, update the timestamps
            if (entry.Entity is ShortIntegerBaseModel intEntity)
            {
                // Update the UpdatedAt timestamp
                intEntity.UpdatedAt = DateTime.UtcNow;

                // If the entity is being added, update the CreatedAt timestamp
                if (entry.State == EntityState.Added)
                {
                    // Set the CreatedAt timestamp to the current time
                    intEntity.CreatedAt = DateTime.UtcNow;
                }
            }
        }

        // Save the changes
        return base.SaveChanges();
    }
    
    // Utility method to convert PascalCase or CamelCase to snake_case
    private string ConvertToSnakeCase(string input)
    {
        // If the string is null or empty, return it as is
        if (string.IsNullOrWhiteSpace(input)) return input;

        // Create a new string builder
        var stringBuilder = new System.Text.StringBuilder();
        
        // Loop through each character in the input string
        for (int i = 0; i < input.Length; i++)
        {
            // If the character is uppercase and not the first character, add an underscore
            if (char.IsUpper(input[i]) && i > 0)
            {
                // Add an underscore
                stringBuilder.Append('_');
            }
            
            // Append the lowercase version of the character
            stringBuilder.Append(char.ToLowerInvariant(input[i]));
        }
        
        // Return the snake_case string
        return stringBuilder.ToString();
    }
    
    // Define the database tables
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
}
