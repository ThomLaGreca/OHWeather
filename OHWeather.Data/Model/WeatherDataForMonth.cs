using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OHWeather.Data.Model
{
  public class WeatherDataForMonth
  {
    public string Month { get; set; }
    [JsonIgnore]
    public DateTime FirstRecordedDate { get; set; }
    [JsonPropertyName("FirstRecordedDate")]
    public string FirstRecordedDateString
    {
      get
      {
        if (FirstRecordedDate == default)
        {
          return "-";
        }
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
        if (LastRecordedDate == default)
        {
          return "-";
        }
        return LastRecordedDate.ToString("yyyy-MM-dd");
      }
    }

    public double TotalRainfall { get; set; }
    public double AverageDailyRainfall { get; set; }

    public int DaysWithNoRainfall { get; set; }
    public int DaysWithRainfall { get; set; }
    public double MedianDailyRainfall { get; set; }
    [JsonIgnore]
    public List<WeatherDataForDay> WeatherDataForDays { get; set; }
  }



}

