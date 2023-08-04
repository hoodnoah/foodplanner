using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
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

      services.AddDbContext<RecipeContext>(Options =>
      {
        Options.UseNpgsql("Host=localhost;Port=5432;Database=recipes_test;Username=test_user;Password=test_password");
      });
    });
  }
}

