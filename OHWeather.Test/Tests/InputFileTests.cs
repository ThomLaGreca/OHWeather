using OHWeather.Processors;
using OHWeather.Test.Constants;
using OHWeather.Test.Utility;
using System.IO;
using Xunit;

namespace OHWeather.Test.Tests
{
  public class InputFileTests
  {
    [Fact]
    public void Return_ValiateFile_Fail()
    {
      string failedPath = "Failed";

      Assert.Throws<FileNotFoundException>(() => FileProcessor.ValidateFilePath(failedPath));
    }

    [Fact]
    public void Return_ValiateFile_Success()
    {
      var successPath = FileLocationUtility.GetRelativeTestPath(FileConstants.TEST_DATA_CSV_ALL);

      FileProcessor.ValidateFilePath(successPath);
    }

    [Fact]
    public void Return_ValiateFile_Fail_FileType()
    {
      var failedPath = FileLocationUtility.GetRelativeTestPath(FileConstants.TEST_DATA_PDF_FAIL);

      Assert.Throws<FileLoadException>(() => FileProcessor.ValidateFilePath(failedPath));
    }


  }
}
