namespace Backend.IntegrationTests;

public class GetRecipesIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
  private readonly HttpClient _client;

  public GetRecipesIntegrationTests(CustomWebApplicationFactory factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task GetRecipes_ReturnsSuccessStatusCode()
  {
    var response = await _client.GetAsync("/recipes");
    response.EnsureSuccessStatusCode();
  }
}