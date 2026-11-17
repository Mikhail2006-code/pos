using System.Globalization;
using System.Text;
using Pos.Application;

namespace Pos.Infrastructure;

public class CsvGradeExporter : IGradeExporter
{
    private readonly string _filePath;

    public CsvGradeExporter(string filePath)
    {
        _filePath = filePath;
    }

    public string Export(IReadOnlyList<FinalGradeRow> rows)
    {
        var builder = new StringBuilder();
        builder.AppendLine("Студент;Итог");

        foreach (var row in rows)
            builder.AppendLine($"{Quote(row.StudentName)};{row.FinalGrade.ToString(CultureInfo.InvariantCulture)}");

        File.WriteAllText(_filePath, builder.ToString(), Encoding.UTF8);
        return Path.GetFullPath(_filePath);
    }

    private static string Quote(string value)
    {
        return "\"" + value.Replace("\"", "\"\"") + "\"";
    }
}