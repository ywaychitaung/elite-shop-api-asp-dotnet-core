namespace elite_shop.Data.Seeders;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbSeeder
{
    public static void SeedAll(ModelBuilder modelBuilder)
    {
        RoleSeeder.Seed(modelBuilder);
    }
}
