using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<RecipeContext>(options => options.UseInMemoryDatabase("recipes"));

var app = builder.Build();

app.MapGet("/recipes", async (RecipeContext db) =>
{
  return await db.Recipes.ToListAsync();
});

app.MapPost("/recipes", async (RecipeContext db, Recipe recipe) =>
{
  db.Recipes.Add(recipe);
  await db.SaveChangesAsync();
  return Results.Created($"/recipes/{recipe.Id}", recipe);
});

app.Run();

public class Recipe
{
  public int Id { get; set; }
  public string? Name { get; set; }

}

public class RecipeContext : DbContext
{
  public RecipeContext(DbContextOptions<RecipeContext> options) : base(options) { }
  public DbSet<Recipe> Recipes { get; set; }
}