namespace Pos.Domain;

public class Rubric
{
    private readonly List<Criterion> _criteria = new();

    public IReadOnlyList<Criterion> Criteria => _criteria;

    public void AddCriterion(Criterion criterion)
    {
        foreach (var existing in _criteria)
        {
            if (string.Equals(existing.Name, criterion.Name, StringComparison.OrdinalIgnoreCase))
                throw new DomainException($"Критерий «{criterion.Name}» уже есть в рубрикаторе.");
        }

        _criteria.Add(criterion);
    }

    public Criterion GetCriterion(Guid criterionId)
    {
        foreach (var criterion in _criteria)
        {
            if (criterion.Id == criterionId)
                return criterion;
        }

        throw new DomainException("Такого критерия нет в рубрикаторе.");
    }
}