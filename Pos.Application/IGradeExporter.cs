namespace Pos.Application;

public interface IGradeExporter
{
    string Export(IReadOnlyList<FinalGradeRow> rows);
}