using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

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
    var recipes = await response.Content.ReadFromJsonAsync<List<Recipe>>() ?? throw new InvalidOperationException("Failed to deserialize result into a list.");
    Assert.Empty(recipes);
  }
}