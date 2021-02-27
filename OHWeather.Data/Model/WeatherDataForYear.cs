using System;
using System.Text.Json.Serialization;

namespace OHWeather.Data.Model
{
  public class WeatherDataForYear
  {
    public int Year { get; set; }
    [JsonIgnore]
    public DateTime FirstRecordedDate { get; set; }
    [JsonPropertyName("FirstRecordedDate")]
    public string FirstRecordedDateString
    {
      get
      {
        return FirstRecordedDate.ToString("yyyy-MM-dd");
      }
    }

    [JsonIgnore]
    public DateTime LastRecordedDate { get; set; }

    [JsonPropertyName("LastRecordedDate")]
    public string LastRecordedDateString
    {
      get
      {
        return LastRecordedDate.ToString("yyyy-MM-dd");
      }
    }

    public double TotalRainfall { get; set; }
    public double AverageDailyRainfall { get; set; }
    public int DaysWithNoRainfall { get; set; }
    public int DaysWithRainfall { get; set; }
    public int LongestNumberOfDaysRaining { get; set; }
    public WeatherDataForMonthRoot MonthlyAggregates { get; set; }

  }

}
