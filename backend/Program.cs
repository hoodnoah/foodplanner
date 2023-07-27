using Microsoft.EntityFrameworkCore;

// Local using
using FoodPlanner.Models;

var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddDbContext<RecipeDb>(opt => opt.UseInMemoryDatabase("RecipesList"));
builder.Services.AddDbContext<RecipeDb>(opt => opt.UseNpgsql(@"Host=localhost;Database=foodplanner;Username=postgres;Password=test_password"));
builder.Services.ConfigureHttpJsonOptions(opt =>
{
  opt.SerializerOptions.WriteIndented = true;
  opt.SerializerOptions.IncludeFields = true;
});

var app = builder.Build();

app.MapGet("/thisWeek", (RecipeService service) => service.GetWeeklyRecipes());
app.MapPost("/addRecipe", async (RecipeDb db, Recipe recipe) =>
{
  await db.Recipes.AddAsync(recipe);
  await db.SaveChangesAsync();
  return Results.Created($"/recipe/{recipe.Id}", recipe);
});

app.Run();

public partial class Program { }
