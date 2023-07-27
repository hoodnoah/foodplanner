using Microsoft.EntityFrameworkCore;

public class Recipe
{
  public int Id { get; set; }
  public string? Name { get; set; }
  public DateTime WeekLastUsed { get; set; }
}

public class RecipeDb : DbContext
{

  // Constructor
  public RecipeDb(DbContextOptions<RecipeDb> options) : base(options) { }

  // Property containing the recipes
  public DbSet<Recipe> Recipes => Set<Recipe>();
}

