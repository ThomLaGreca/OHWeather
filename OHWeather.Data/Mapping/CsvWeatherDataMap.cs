using CsvHelper.Configuration;
using OHWeather.Data.Model;

namespace OHWeather.Data.Mapping
{
  public class CsvWeatherDataMap : ClassMap<CsvWeatherData>
  {
    public CsvWeatherDataMap()
    {
      Map(t => t.ProductCode).Name("Product code");
      Map(t => t.StationNumber).Name("Bureau of Meteorology station number");
      Map(t => t.Year);
      Map(t => t.Month);
      Map(t => t.Day);
      Map(t => t.RainfallAmount).Default(0).Name("Rainfall amount (millimetres)");
      Map(t => t.RainfallPeriod).Default(0).Name("Period over which rainfall was measured (days)");
      Map(t => t.Quality);
    }
  }
}
