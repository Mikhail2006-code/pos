namespace Pos.Domain;

public interface IRubricRepository
{
    Rubric Get();
    void Save(Rubric rubric);
}