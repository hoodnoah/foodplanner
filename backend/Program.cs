using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<RecipeContext>(options => options.UseNpgsql(@"Host=localhost;Username=test_user;Password=test_password;Database=recipes"));

var app = builder.Build();

app.MapGet("/recipes", async (RecipeContext db) =>
{
  return await db.Recipes.ToListAsync();
});

app.MapPost("/recipes", async (RecipeContext db, RecipeDTO recipe) =>
{
  Recipe newRecipe = new Recipe { Name = recipe.Name };
  db.Recipes.Add(newRecipe);
  await db.SaveChangesAsync();
  return Results.Created($"/recipes/{newRecipe.Id}", recipe);
});

app.Run();

public partial class Program { }