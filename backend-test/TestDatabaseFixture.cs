public class TestDatabaseFixture
{
  private const string ConnectionString = @"Host=localhost;Database=recipes_test;Username=test_user;Password=test_password";

  private static readonly object _lock = new();

  private static bool _databaseInitialized;

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
            new Recipe { Name = "Recipe 1", WeekLastUsed = DateTime.Now },
            new Recipe { Name = "Recipe 2", WeekLastUsed = DateTime.Now }
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