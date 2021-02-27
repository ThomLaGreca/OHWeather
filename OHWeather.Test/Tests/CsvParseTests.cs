using OHWeather.Data.Model;
using OHWeather.Processors;
using OHWeather.Test.Constants;
using OHWeather.Test.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace OHWeather.Test.Tests
{
  public class CsvParseTests
  {
    [Fact]
    public void Return_ParseCsv_Fail_NoRecords()
    {
      var failedPath = FileLocationUtility.GetRelativeTestPath(FileConstants.TEST_DATA_CSV_NONE);

      Assert.Throws<InvalidDataException>(() => WeatherDataProcessor.ParseCsv(failedPath));
    }

    [Fact]
    public void Return_ParseCsv_Success_Single()
    {
      var failedPath = FileLocationUtility.GetRelativeTestPath(FileConstants.TEST_DATA_CSV_SINGLE);

      Assert.Single(WeatherDataProcessor.ParseCsv(failedPath));
    }

    [Fact]
    public void Return_ParseCsv_Success_All()
    {
      var failedPath = FileLocationUtility.GetRelativeTestPath(FileConstants.TEST_DATA_CSV_ALL);

      Assert.Equal(59413, WeatherDataProcessor.ParseCsv(failedPath).Count);
    }

    [Fact]
    public void Return_ProcessRows_Success_Single()
    {
      // Expected 

      //  IDCJAC0009,066062,1969,04,16,103.9,1,Y

      var expectedWeatherData = new WeatherDataRoot()
      {
        WeatherData = new WeatherData()
        {
          WeatherDataForYear = new List<WeatherDataForYear>()
          {
            new WeatherDataForYear
            {
              Year = 1969,
              FirstRecordedDate = new DateTime(1969, 4, 16),
              LastRecordedDate = new DateTime(1969, 4, 16),
              TotalRainfall = 103.9,
              AverageDailyRainfall = 103.9,
              DaysWithNoRainfall = 0,
              DaysWithRainfall = 1,
              LongestNumberOfDaysRaining = 1,
              MonthlyAggregates = new WeatherDataForMonthRoot()
              {
                WeatherDataForMonth = new List<WeatherDataForMonth>(){
                  new WeatherDataForMonth()
                  {
                    Month = "April",
                    FirstRecordedDate = new DateTime(1969, 4, 16),
                    LastRecordedDate = new DateTime(1969, 4, 16),
                    TotalRainfall = 103.9,
                    AverageDailyRainfall = 103.9,
                    MedianDailyRainfall = 103.9,
                    DaysWithNoRainfall = 0,
                    DaysWithRainfall = 1,
                    WeatherDataForDays = new List<WeatherDataForDay>()
                    {
                      new WeatherDataForDay()
                      {
                        Day = 16,
                        RainfallAmount = 103.9
                      }
                    }
                  }
                }
              }
            }
          }
        }
      };



      var testCsvWeatherData = new List<CsvWeatherData>()
      {
        new CsvWeatherData()
        {
          ProductCode = "IDCJAC0009",
          StationNumber = "066062",
          Year = 1969,
          Month = 04,
          Day = 16,
          RainfallAmount = 103.9,
          RainfallPeriod = 1
        }
      };

      var result = WeatherDataProcessor.ProcessRows(testCsvWeatherData);

      string expected = JsonSerializer.Serialize(expectedWeatherData);

      string actual = JsonSerializer.Serialize(result);

      Assert.Equal(expected, actual);
    }

    [Fact]
    public void Return_ProcessRows_CalculateRainfallTotal_Success_December2020()
    {

      var failedPath = FileLocationUtility.GetRelativeTestPath(FileConstants.TEST_DATA_CSV_2020);

      var result = WeatherDataProcessor.Process(failedPath);

      var year2020 = result.WeatherData.WeatherDataForYear.FirstOrDefault(t => t.Year == 2020);

      Assert.Equal(1140.2, year2020.TotalRainfall);
    }

    [Fact]
    public void Return_ProcessRows_CalculateLongestNumberOfDaysRaining_Success()
    {
      var failedPath = FileLocationUtility.GetRelativeTestPath(FileConstants.TEST_DATA_CSV_2020);

      var result = WeatherDataProcessor.Process(failedPath);

      var year2020 = result.WeatherData.WeatherDataForYear.FirstOrDefault(t => t.Year == 2020);

      Assert.Equal(9, year2020.LongestNumberOfDaysRaining);
    }
  }
}
