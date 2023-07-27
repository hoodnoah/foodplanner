// system libraries
using System.Globalization;

using FoodPlanner.Models;

public class RecipeService
{
  private readonly RecipeDb? _db;

  public RecipeService(RecipeDb db)
  {
    _db = db;
  }

  public List<Recipe> GetWeeklyRecipes()
  {
    // error out if db context not initialized
    if (_db == null)
    {
      throw new InvalidOperationException("Tried to access a database connection that was not initialized.");
    }

    // establish the current week of the year
    int currentWeek = GetCurrentWeek();

    List<Recipe> current_recipes = _db.Recipes.Where(r => r.WeekLastUsed == currentWeek).ToList();
    if (current_recipes.Count() != 0)
    {
      return current_recipes;
    }
    else
    {
      // randomly select 7 recipes which haven't been used in the last week
      Random rng = new Random();

      List<Recipe> valid_recipes = _db.Recipes.Where(r => r.WeekLastUsed != null && r.WeekLastUsed < currentWeek - 1).ToList();
      List<Recipe> shuffled_recipes = Shuffle(valid_recipes);

      return shuffled_recipes.Take(7).ToList();
    }
  }

  /// <summary>
  /// Randomly shuffles a list of recipes.
  /// </summary>
  /// <param name="recipes">The list of recipes to shuffle.</param>
  /// <returns>The shuffled list of recipes.</returns>
  private static List<Recipe> Shuffle(List<Recipe> recipes)
  {
    Random rng = new Random();
    int n = recipes.Count;
    while (n > 1)
    {
      n--;
      int k = rng.Next(n + 1);
      Recipe value = recipes[k];
      recipes[k] = recipes[n];
      recipes[n] = value;
    }
    return recipes;
  }

  private int GetCurrentWeek()
  {
    return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Saturday);
  }


}