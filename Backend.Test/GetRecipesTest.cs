using System.Net.Http.Json;

namespace Backend.IntegrationTests;

public class GetRecipesIntegrationTests : IClassFixture<CustomWebApplicationFactory>, IDisposable
{
  private readonly HttpClient _client;
  private readonly CustomWebApplicationFactory _factory;

  public GetRecipesIntegrationTests(CustomWebApplicationFactory factory)
  {
    _factory = factory;
    _client = factory.CreateClient();
  }

  // Clean up the database after each test
  public void Dispose()
  {
    _factory.DeleteAllRecords();
  }


  [Fact]
  public async Task GetRecipes_ReturnsSuccessStatusCode()
  {
    var response = await _client.GetAsync("/recipes");
    response.EnsureSuccessStatusCode();
  }

  [Fact]
  public async Task GetRecipes_Always_ReturnsEmptyListWhenNoRecipes()
  {
    var response = await _client.GetAsync("/recipes");
    var recipes = await response.Content.ReadFromJsonAsync<List<Recipe>>();
    Assert.NotNull(recipes);
    Assert.Empty(recipes);
  }

  [Fact]
  public async Task PostRecipe_Always_ReturnsCreatedRecipe()
  {
    var recipe = new Recipe { Name = "Test Recipe" };
    var response = await _client.PostAsJsonAsync("/recipes", recipe);
    response.EnsureSuccessStatusCode();
    var createdRecipe = await response.Content.ReadFromJsonAsync<Recipe>();
    Assert.NotNull(createdRecipe);
    Assert.Equal(recipe.Name, createdRecipe.Name);
  }
}