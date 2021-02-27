using OHWeather.Processors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace OHWeather
{
  class Program
  {
    static void Main(string[] args)
    {
      Run();

      while (Console.ReadKey().Key == ConsoleKey.Enter) { Console.Clear(); Run(); }
    }
    private static void Run()
    {
      var fileLocation = InitDialog();

      Console.Clear();

      Console.WriteLine($"Beginning converstion for file: {fileLocation}");

      var result = WeatherDataProcessor.Process(fileLocation);

      var jsonResult = JsonSerializer.Serialize(result, new JsonSerializerOptions() { WriteIndented = true });

      PrintSuccessInfo();

      var input = Console.ReadKey();
      Console.WriteLine();

      if (input.KeyChar == 'y')
      {
        FileProcessor.WriteDataToFile(jsonResult, $"{Path.GetFileNameWithoutExtension(fileLocation)}_results.json");
      }

      if (input.KeyChar == 'n')
      {
        Console.WriteLine(jsonResult);
      }

      Console.WriteLine();
      Console.WriteLine("Press enter to restart.");
      Console.WriteLine();
    }

    private static string InitDialog()
    {
      PrintGreeting();

      return PromptFileSelection();
    }

    private static void ShowOptions(List<string> fileOptions)
    {
      Console.WriteLine();
      Console.WriteLine("Please select the file you wish to input.");
      Console.WriteLine();

      int count = 0;

      foreach (var file in fileOptions)
      {
        Console.WriteLine($"{++count}. {file}");
      }

      Console.WriteLine();
      Console.WriteLine("-- -- -- -- -- -- -- -- -- -- -- -- -- -- --");
      Console.WriteLine();
    }

    private static string PromptFileSelection()
    {

      var files = FileProcessor.ReadFiles();

      ShowOptions(files);

      int numSelection = 0;

      while (numSelection == 0)
      {
        var selection = Console.ReadLine();

        if (!int.TryParse(selection, out numSelection))
        {
          numSelection = 0;
          Console.WriteLine("\nCould not parse selection as a number. Please try again.\n\n");
          PromptFileSelection();
          //continue;
        }

        if (numSelection <= 0)
        {
          numSelection = 0;
          Console.WriteLine("\nSelection must be a positive number. Please try again.\n\n");
          PromptFileSelection();
          //continue;
        }

        if (numSelection > files.Count)
        {
          numSelection = 0;
          Console.WriteLine($"\nSelection is out of range. Selection must be between 1 and {files.Count}. Please try again.\n\n");
          PromptFileSelection();
          // continue;
        }

      }

      return files.ElementAt(numSelection - 1);

    }

    private static void PrintGreeting()
    {
      Console.WriteLine();
      Console.WriteLine("Welcome to the Observatory Hill Weather Station!");
      Console.WriteLine();

      Console.WriteLine("-- -- In order to input files into OHWeather they must be located in the appropriate FileBucket. -- -- ");
      Console.WriteLine("If running the application using visual studio you can add the file to ./OHWeather/FileBucket. (Making sure to select 'Copy to output folder - always/if newer' in the files properties.)");
      Console.WriteLine("If running the application via the .exe simmilarly the files will need to be placed in the {root}/FileBucket folder.");

      Console.WriteLine();
    }

    private static void PrintSuccessInfo()
    {
      Console.WriteLine();
      Console.WriteLine("Success! Observatory Hill Weather has completed your weather data conversion!");

      Console.WriteLine();
      Console.WriteLine("The converted data can sometimes be too large for the 'Screen buffer size' of the console window.");
      Console.WriteLine();
      Console.WriteLine(" -- Select 'y' if you would like to save the results to .JSON file. ");
      Console.WriteLine(" -- Select 'n' to continue printing the results to the console window.");
    }
  }
}
