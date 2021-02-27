using System;
using System.IO;
using System.Reflection;

namespace OHWeather.Test.Utility
{
  public static class FileLocationUtility
  {
    public static string GetRelativeTestPath(string fileName)
    {

      var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
      var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
      var relativePath = Path.GetDirectoryName(codeBasePath);

      return $"{relativePath}{fileName}";
    }
  }
}
