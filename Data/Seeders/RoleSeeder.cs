namespace elite_shop.Data.Seeders;

using elite_shop.Models.Domains;
using Microsoft.EntityFrameworkCore;

public class RoleSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Customer", Description = "Customers" },
            new Role { Id = 2, Name = "Admin", Description = "Admins" }
        );
    }
}
