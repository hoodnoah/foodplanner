public class Helpers
{
  public Helpers()
  {

  }

  public DateTime GetFirstDayOfWeek(DateTime date)
  {
    var firstDayOfWeek = date.AddDays(-(int)date.DayOfWeek);
    return firstDayOfWeek;
  }

}