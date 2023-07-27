namespace Backend.Tests;

using Microsoft.EntityFrameworkCore;
using FoodPlanner.Models;

public class TestDatabaseFixture
{
  private const string ConnectionString = @"Host=localhost;Database=foodplanner;Password=test_password;Username=test_user";
  private static readonly object _lock = new();
  private static bool _databaseInitialized;

  // constructor
  public TestDatabaseFixture()
  {
    lock (_lock)
    {
      if (!_databaseInitialized)
      {
        using (var context = CreateContext())
        {
          context.Database.EnsureDeleted();
          context.Database.EnsureCreated();

          context.AddRange(
              new Recipe { Name = "Recipe 1", WeekLastUsed = DateTime.Now.ToUniversalTime() },
              new Recipe { Name = "Recipe 2", WeekLastUsed = DateTime.Now.ToUniversalTime() },
              new Recipe { Name = "Recipe 3", WeekLastUsed = DateTime.Now.ToUniversalTime() }
          );

          context.SaveChanges();
        }

        _databaseInitialized = true;
      }
    }
  }

  public RecipeDb CreateContext()
    => new RecipeDb(
        new DbContextOptionsBuilder<RecipeDb>()
            .UseNpgsql(ConnectionString)
            .Options
    );
}

public class RecipeDbTest : IClassFixture<TestDatabaseFixture>
{
  public RecipeDbTest(TestDatabaseFixture fixture) => Fixture = fixture;

  public TestDatabaseFixture Fixture { get; }

  [Fact]
  public void GetRecipes()
  {
    using var context = Fixture.CreateContext();
    var recipes = context.Recipes.ToList();
    Assert.Equal(3, recipes.Count);
  }

}