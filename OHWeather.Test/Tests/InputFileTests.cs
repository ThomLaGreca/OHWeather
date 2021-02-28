using OHWeather.Test.Constants;
using OHWeather.Test.Utility;
using OHWeather.Utility;
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

      Assert.Throws<FileNotFoundException>(() => FileUtility.ValidateFilePath(failedPath));
    }

    [Fact]
    public void Return_ValiateFile_Success()
    {
      var successPath = FileLocationUtility.GetRelativeTestPath(FileConstants.TEST_DATA_CSV_ALL);

      FileUtility.ValidateFilePath(successPath);
    }

    [Fact]
    public void Return_ValiateFile_Fail_FileType()
    {
      var failedPath = FileLocationUtility.GetRelativeTestPath(FileConstants.TEST_DATA_PDF_FAIL);

      Assert.Throws<FileLoadException>(() => FileUtility.ValidateFilePath(failedPath));
    }


  }
}
