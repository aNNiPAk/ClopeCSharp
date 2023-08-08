using System.Collections;

namespace Infrastructure.Data;

public class FileDataReader : IEnumerable<string>
{
    private readonly string _filePath;

    public FileDataReader(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("Путь к файлу не может быть пустым или содержать только пробелы.",
                nameof(filePath));
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Файл не найден.", filePath);
        }

        _filePath = filePath;
    }


    public IEnumerator<string> GetEnumerator()
    {
        using var reader = new StreamReader(_filePath);

        while (reader.ReadLine() is { } line)
        {
            yield return line;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}