using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
{
  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.ConfigureAppConfiguration((context, config) =>
    {
    });

    builder.ConfigureServices(services =>
    {
      // Remove extant dbservice, if one exists
      var descriptor = services.SingleOrDefault(
         d => d.ServiceType ==
             typeof(DbContextOptions<RecipeContext>));

      if (descriptor != null)
      {
        services.Remove(descriptor);
      }

      // Add dbContext
      services.AddDbContext<RecipeContext>(Options =>
      {
        Options.UseNpgsql("Host=localhost;Port=5432;Database=recipes_test;Username=test_user;Password=test_password");
      });

      // Create an intermediate service provider
      var serviceProvider = services.BuildServiceProvider();

      // Create a scope to get reference to the dbContext
      using var scope = serviceProvider.CreateScope();
      var scopedService = scope.ServiceProvider;
      var db = scopedService.GetRequiredService<RecipeContext>();

      // Ensure db is created
      db.Database.EnsureCreated();

      // Run migrations
      try
      {
        db.Database.Migrate();
      }
      catch (Exception)
      {
        Console.Error.WriteLine("Failed to migrate database");
      }
    });
  }

  /// <summary>
  /// Cleans up the database by deleting all records.
  /// Does not delete the database itself since the schema
  /// should not change between uses.
  /// </summary>
  public void DeleteAllRecords()
  {
    using var scope = Services.CreateScope();
    var scopedService = scope.ServiceProvider;
    var db = scopedService.GetRequiredService<RecipeContext>();

    db.Recipes.RemoveRange(db.Recipes);
    db.SaveChanges();
  }

  public void CleanUp()
  {
    using var scope = Services.CreateScope();
    var scopedService = scope.ServiceProvider;
    var db = scopedService.GetRequiredService<RecipeContext>();

    db.Database.EnsureDeleted();
  }
}

