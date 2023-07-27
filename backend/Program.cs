var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddDbContext<RecipeDb>(options => options.UseInMemoryDatabase("RecipeDatabase"));
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
