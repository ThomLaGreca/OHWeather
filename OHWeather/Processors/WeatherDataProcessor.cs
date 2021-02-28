using CsvHelper;
using OHWeather.Core.Extensions;
using OHWeather.Data.Mapping;
using OHWeather.Data.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace OHWeather.Processors
{
  public static class WeatherDataProcessor
  {
    public static WeatherDataRoot Process(string fileName)
    {
      try
      {
        var rows = ParseCsv(fileName);

        return ProcessRows(rows);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        throw;
      }
    }

    public static WeatherDataRoot ProcessRows(List<CsvWeatherData> rows)
    {
      var rowsByYear = rows
        .Where(t => t.Year <= DateTime.Now.Year)
        .GroupBy(t => t.Year)
        .ToList();

      Console.WriteLine($"Processing {rowsByYear.Count} 'Year' records.");

      var result = new WeatherDataRoot()
      {
        WeatherData = new WeatherData()
        {
          WeatherDataForYear = new List<WeatherDataForYear>()
        }
      };

      foreach (var group in rowsByYear)
      {
        WeatherDataForYear year = CalculateWeatherDataForYear(group);
        result.WeatherData.WeatherDataForYear.Add(year);
      }

      return result;
    }

    public static WeatherDataForYear CalculateWeatherDataForYear(IGrouping<int, CsvWeatherData> group)
    {
      var currentYear = new WeatherDataForYear()
      {
        Year = group.Key,
        TotalRainfall = 0,
        AverageDailyRainfall = 103.9,
        DaysWithNoRainfall = 0,
        DaysWithRainfall = 0,
        LongestNumberOfDaysRaining = 0,
        MonthlyAggregates = new WeatherDataForMonthRoot()
        {
          WeatherDataForMonth = new List<WeatherDataForMonth>()
        }
      };

      int runningDaysOfRain = 0;

      foreach (var item in group)
      {
        var itemDate = new DateTime(item.Year, item.Month, item.Day);

        if (itemDate > DateTime.Now)
        {
          continue;
        }

        if (item.RainfallAmount > 0 && (itemDate < currentYear.FirstRecordedDate || currentYear.FirstRecordedDate == default))
        {
          currentYear.FirstRecordedDate = itemDate;
        }

        if (item.RainfallAmount > 0 && (itemDate > currentYear.LastRecordedDate || currentYear.LastRecordedDate == default))
        {
          currentYear.LastRecordedDate = itemDate;
        }

        if (item.RainfallAmount == 0)
        {
          ++currentYear.DaysWithNoRainfall;

          if (runningDaysOfRain > currentYear.LongestNumberOfDaysRaining)
          {
            currentYear.LongestNumberOfDaysRaining = runningDaysOfRain;
          }

          runningDaysOfRain = 0;
        }

        if (item.RainfallAmount > 0)
        {
          ++currentYear.DaysWithRainfall;
          currentYear.TotalRainfall = Math.Round(currentYear.TotalRainfall + item.RainfallAmount, 1);

          ++runningDaysOfRain;
        }

        if (item.RainfallPeriod > currentYear.LongestNumberOfDaysRaining)
        {
          currentYear.LongestNumberOfDaysRaining = item.RainfallPeriod;
        }

        currentYear.AverageDailyRainfall = Math.Round(currentYear.TotalRainfall / (currentYear.DaysWithRainfall + currentYear.DaysWithNoRainfall), 3);

        currentYear.MonthlyAggregates.WeatherDataForMonth = CalculateWeatherDataForMonth(currentYear.MonthlyAggregates.WeatherDataForMonth, itemDate, item);

      }

      return currentYear;
    }

    private static List<WeatherDataForMonth> CalculateWeatherDataForMonth(List<WeatherDataForMonth> monthlyAggregates, DateTime itemDate, CsvWeatherData item)
    {
      string monthName = itemDate.GetMonthName();

      var currentMonth = monthlyAggregates.FirstOrDefault(t => string.Equals(t.Month, monthName, StringComparison.OrdinalIgnoreCase));

      if (currentMonth == null)
      {
        currentMonth = new WeatherDataForMonth()
        {
          Month = monthName,
          TotalRainfall = 0,
          AverageDailyRainfall = 0,
          MedianDailyRainfall = 0,
          DaysWithNoRainfall = 0,
          DaysWithRainfall = 0,
          WeatherDataForDays = new List<WeatherDataForDay>()
        };

        monthlyAggregates.Add(currentMonth);
      }

      if (item.RainfallAmount > 0 && (itemDate < currentMonth.FirstRecordedDate || currentMonth.FirstRecordedDate == default))
      {
        currentMonth.FirstRecordedDate = itemDate;
      }

      if (item.RainfallAmount > 0 && (itemDate > currentMonth.LastRecordedDate || currentMonth.LastRecordedDate == default))
      {
        currentMonth.LastRecordedDate = itemDate;
      }

      if (item.RainfallAmount == 0)
      {
        ++currentMonth.DaysWithNoRainfall;
      }

      if (item.RainfallAmount > 0)
      {
        ++currentMonth.DaysWithRainfall;
        currentMonth.TotalRainfall = Math.Round(currentMonth.TotalRainfall + item.RainfallAmount, 1);
      }

      int daysInMonth = currentMonth.DaysWithRainfall + currentMonth.DaysWithNoRainfall;

      currentMonth.AverageDailyRainfall = Math.Round(currentMonth.TotalRainfall / daysInMonth, 3);

      currentMonth.WeatherDataForDays.Add(new WeatherDataForDay() { Day = item.Day, RainfallAmount = item.RainfallAmount });

      currentMonth.MedianDailyRainfall = CalculateMedian(currentMonth.WeatherDataForDays);

      return monthlyAggregates;
    }

    public static double CalculateMedian(List<WeatherDataForDay> weatherDataForDays)
    {
      var orderedList = weatherDataForDays.OrderBy(t => t.RainfallAmount).ToList();

      int count = orderedList.Count;

      if (count == 0)
      {
        throw new InvalidDataException("Unable to calculate median. Collection is empty.");
      }

      if (count == 1)
      {
        return orderedList.First().RainfallAmount;
      }

      if (count % 2 == 0)
      {
        int leftIndex = (count / 2) - 1;
        int rightIndex = count / 2;

        return (orderedList[leftIndex].RainfallAmount + orderedList[rightIndex].RainfallAmount) / 2;
      }
      else
      {
        return orderedList[count / 2].RainfallAmount;
      }

    }

    public static List<CsvWeatherData> ParseCsv(string fileName)
    {

      Stopwatch stopWatch = new Stopwatch();

      using var reader = new StreamReader(fileName, Encoding.Default);
      using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

      csv.Context.RegisterClassMap<CsvWeatherDataMap>();

      stopWatch.Start();

      var records = csv.GetRecords<CsvWeatherData>().ToList();

      stopWatch.Stop();

      if (records.Any())
      {
        Console.WriteLine($"{records.Count} records found in file. Elapsed: {stopWatch.ElapsedMilliseconds}ms.");
      }
      else
      {
        throw new InvalidDataException("No records were found in the provided csv file.");
      }


      return records;
    }


  }
}
