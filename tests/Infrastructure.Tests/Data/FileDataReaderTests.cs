using Infrastructure.Data;

namespace Infrastructure.Tests.Data;

public class FileDataReaderTests
{
    [Fact]
    public void Constructor_ThrowsArgumentException_WhenFilePathIsNullOrWhiteSpace()
    {
        // Arrange
        string filePath = null;

        // Act and Assert
        Assert.Throws<ArgumentException>(() => new FileDataReader(filePath));
    }

    [Fact]
    public void Constructor_ThrowsFileNotFoundException_WhenFileDoesNotExist()
    {
        // Arrange
        string filePath = "nonexistent.txt";

        // Act and Assert
        Assert.Throws<FileNotFoundException>(() => new FileDataReader(filePath));
    }

    [Fact]
    public void GetEnumerator_ReturnsExpectedLines()
    {
        // Arrange
        string filePath = "test.txt";
        File.WriteAllLines(filePath, new[] { "line1", "line2" });
        var fileDataReader = new FileDataReader(filePath);

        // Act
        using var lines = fileDataReader.GetEnumerator();

        // Assert
        Assert.Equal("line1", lines.Current);
        lines.MoveNext();
        Assert.Equal("line2", lines.Current);

        // Clean up
        File.Delete(filePath);
    }
}