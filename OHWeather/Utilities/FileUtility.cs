using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OHWeather.Utility
{
  public static class FileUtility
  {
    public static List<string> ReadFiles()
    {
      string path = "./FileBucket";

      Console.WriteLine($"Reading Files from {path}.");

      var files = Directory
        .EnumerateFiles(path, "*", SearchOption.AllDirectories)
        .Where(s => s.EndsWith(".csv") || s.EndsWith(".xlsx"))
        .ToList();

      ValidateFiles(files, path);

      return files;
    }

    public static void ValidateFilePath(string fileName)
    {
      Console.WriteLine($"Validating file: {fileName}");

      if (!File.Exists(fileName))
      {
        throw new FileNotFoundException();
      }

      if (Path.GetExtension(fileName) != ".csv" && Path.GetExtension(fileName) != ".xlsx")
      {
        throw new FileLoadException("File does not conform to the required .csv or .xlsx file format.");
      }

      Console.WriteLine($"Validation successful for file: {fileName}");
    }

    public static void ValidateFiles(List<string> files, string path)
    {
      Console.WriteLine($"Validating files in directory: {path}");

      if (!files.Any())
      {
        Console.WriteLine("No files found in FileBucket. Please add a file to the specified location and restart the application.");
        throw new FileNotFoundException();
      }

      Console.WriteLine($"Validation successful for file directory: {path}");
    }

    public static void WriteDataToFile(string jsonResult, string fileName)
    {
      try
      {
        string path = $"{Directory.GetCurrentDirectory()}/Results";

        Directory.CreateDirectory(path);

        File.WriteAllText($"{path}/{fileName}", jsonResult);

        Console.WriteLine($"Successfully saved json to file location: {path}/{fileName}");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Unable to write Json to file. {ex}");
        throw;
      }

    }
  }
}
