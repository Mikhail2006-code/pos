using Pos.Domain;

namespace Pos.Infrastructure;

public class InMemoryEvaluationRepository : IEvaluationRepository
{
    private readonly List<Evaluation> _evaluations = new();

    public void Save(Evaluation evaluation)
    {
        Remove(evaluation.StudentId, evaluation.CriterionId);
        _evaluations.Add(evaluation);
    }

    public void Remove(Guid studentId, Guid criterionId)
    {
        _evaluations.RemoveAll(e => e.StudentId == studentId && e.CriterionId == criterionId);
    }

    public IReadOnlyList<Evaluation> GetByStudent(Guid studentId)
    {
        var result = new List<Evaluation>();
        foreach (var evaluation in _evaluations)
        {
            if (evaluation.StudentId == studentId)
                result.Add(evaluation);
        }

        return result;
    }
}