using System;

namespace OHWeather.Core.Extensions
{
  public static class DateTimeExtensions
  {
    public static string GetMonthName(this DateTime dt)
    {
      return dt.ToString("MMMM");
    }
  }
}
