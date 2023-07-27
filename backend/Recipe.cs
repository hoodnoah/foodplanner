using Microsoft.EntityFrameworkCore;

namespace FoodPlanner.Models
{
  public class Recipe
  {
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? WeekLastUsed { get; set; }
  }


  public class RecipeDb : DbContext
  {
    public RecipeDb(DbContextOptions<RecipeDb> options) : base(options) { }

    public DbSet<Recipe> Recipes => Set<Recipe>();
  }

}