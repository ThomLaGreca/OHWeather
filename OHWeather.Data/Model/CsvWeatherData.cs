namespace OHWeather.Data.Model
{
  public class CsvWeatherData
  {
    public string ProductCode { get; set; }
    public string StationNumber { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
    public double RainfallAmount { get; set; }
    public int RainfallPeriod { get; set; }
    public string Quality { get; set; }
  }
}
