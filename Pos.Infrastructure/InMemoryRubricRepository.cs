using Pos.Domain;

namespace Pos.Infrastructure;

public class InMemoryRubricRepository : IRubricRepository
{
    private Rubric _rubric = new();

    public Rubric Get()
    {
        return _rubric;
    }

    public void Save(Rubric rubric)
    {
        _rubric = rubric;
    }
}