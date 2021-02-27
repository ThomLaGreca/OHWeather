using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OHWeather.Data.Model
{
  public class WeatherData
  {
    public List<WeatherDataForYear> WeatherDataForYear { get; set; }
  }
}
