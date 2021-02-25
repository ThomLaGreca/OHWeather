using System;
using System.Collections.Generic;

namespace OHWeather.Data.Model
{
  public class WeatherDataForYear
  {
    public string Year { get; set; }
    public DateTime FirstRecordedDate { get; set; }
    public DateTime LastRecordedDate { get; set; }
    public double TotalRainfall { get; set; }
    public double AverageDailyRainfall { get; set; }
    public int DaysWithNoRainfall { get; set; }
    public int DaysWithRainfall { get; set; }
    public int LongestNumberOfDaysRaining { get; set; }
    public List<WeatherDataForMonth> MonthlyAggregates { get; set; }

  }
}
