namespace backend_test;

public class RecipeTest : IClassFixture<RecipeDb>
{
  public RecipeTest(RecipeDb fixture)
      => fixture = fixture;
}

public class UnitTest1
{
  [Fact]
  public void Test1()
  {
    using var context = TestDatabaseFixture.CreateContext();
    var recipes = new RecipeDb(context);

    var recipe = recipes.GetRecipe(1);
    Assert.Equal("Recipe 1", recipe.Name);

  }
}