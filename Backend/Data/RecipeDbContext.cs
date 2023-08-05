using Microsoft.EntityFrameworkCore;

public class RecipeContext : DbContext
{
  public RecipeContext(DbContextOptions<RecipeContext> options) : base(options) { }
  public DbSet<Recipe> Recipes { get; set; }
}