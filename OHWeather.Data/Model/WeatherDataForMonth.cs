using System;

namespace OHWeather.Data.Model
{
  public class WeatherDataForMonth
  {
    public string Month { get; set; }
    public DateTime FirstRecordedDate { get; set; }
    public DateTime LastRecordedDate { get; set; }
    public double TotalRainfall { get; set; }
    public double AverageDailyRainfall { get; set; }
    public double MedianDailyRainfall { get; set; }
    public int DaysWithNoRainfall { get; set; }
    public int DaysWithRainfall { get; set; }

  }
}
