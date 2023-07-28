using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Test;

public class GetRecipesTest
{
  [Fact]
  public async Task GetAllRecipes_ReturnsEmptyList_WhenNoRecipesExist()
  {
    // Arrange
    await using var application = new WebApplicationFactory<Program>();
    var client = application.CreateClient();

    // Act
    var response = await client.GetAsync("/recipes");

    // Assert
    // Should be an empty list of recipes since the database contains none
    response.EnsureSuccessStatusCode();
    var recipes = await response.Content.ReadFromJsonAsync<List<Recipe>>()
      ?? throw new InvalidOperationException("Failed to deserialize result into a list.");
    Assert.Empty(recipes);
  }

  [Fact]
  public async Task GetAllRecipes_ReturnsAllRecipes()
  {
    // Arrange
    var applicationFactory = new WebApplicationFactory<Program>()
      .WithWebHostBuilder(builder =>
      {
        builder.ConfigureServices(services =>
        {
          var descriptor = services.SingleOrDefault(
            d => d.ServiceType ==
              typeof(DbContextOptions<RecipeContext>)
          );

          services.Remove(descriptor);
          services.AddDbContext<RecipeContext>(options =>
          {
            options.UseInMemoryDatabase("recipes_test");
          });
        });
      });

    using var scope = applicationFactory.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<RecipeContext>();

    dbContext.Recipes.Add(new Recipe { Name = "Recipe 1" });
    dbContext.Recipes.Add(new Recipe { Name = "Recipe 2" });
    dbContext.SaveChanges();

    var client = applicationFactory.CreateClient();

    // Act
    var response = await client.GetAsync("/recipes");

    // Assert
    response.EnsureSuccessStatusCode();
    var recipes = await response.Content.ReadFromJsonAsync<List<Recipe>>();

    Assert.NotNull(recipes);
    Assert.Equal(2, recipes.Count);
    Assert.Equal("Recipe 1", recipes[0].Name);
    Assert.Equal("Recipe 2", recipes[1].Name);
  }
}