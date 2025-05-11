using Hotels.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace Hotels.Unit.Tests.Services;

public class StaticFilesServiceTests
{
    private readonly Mock<ILogger<StaticFilesService>> _loggerMock;
    private readonly StaticFilesService _staticFilesService;

    public StaticFilesServiceTests()
    {
        _loggerMock = new Mock<ILogger<StaticFilesService>>();
        _staticFilesService = new StaticFilesService(_loggerMock.Object);
    }

    #region SaveFileAsync

    [Fact]
    public async Task SaveFileAsync_SavesFileAndReturnFilePath()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var fileName = "test.txt";
        var directoryPath = "TestDirectory";
        var filePath = Path.Combine(directoryPath, fileName);

        var fileContent = "Test content";
        var memoryStream = new MemoryStream();
        var writer = new StreamWriter(memoryStream);
        writer.Write(fileContent);
        writer.Flush();
        memoryStream.Position = 0;

        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default))
            .Callback<Stream, CancellationToken>((stream, _) =>
            {
                memoryStream.CopyTo(stream);
            })
            .Returns(Task.CompletedTask);

        // Act
        var result = await _staticFilesService.SaveFileAsync(fileMock.Object, directoryPath);

        // Assert
        Assert.Equal(filePath, result);
        Assert.True(File.Exists(filePath));

        // Cleanup
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        if (Directory.Exists(directoryPath))
        {
            Directory.Delete(directoryPath);
        }
    }

    #endregion

    #region Remove

    [Fact]
    public void Remove_DeletesFileAndReturnsTrue()
    {
        // Arrange
        var directoryPath = "TestDirectory";
        var fileName = "test.txt";
        var filePath = Path.Combine(directoryPath, fileName);

        Directory.CreateDirectory(directoryPath);
        File.WriteAllText(filePath, "Test content");

        // Act
        var result = _staticFilesService.Remove(filePath);

        // Assert
        Assert.True(result);
        Assert.False(File.Exists(filePath));

        // Cleanup
        if (Directory.Exists(directoryPath))
        {
            Directory.Delete(directoryPath);
        }
    }

    [Fact]
    public void Remove_FileDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var filePath = "NonExistentFile.txt";

        // Act
        var result = _staticFilesService.Remove(filePath);

        // Assert
        Assert.False(result);
    }

    #endregion
}
