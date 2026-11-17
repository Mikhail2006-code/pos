namespace Pos.Domain;

public interface IEvaluationRepository
{
    void Save(Evaluation evaluation);
    void Remove(Guid studentId, Guid criterionId);
    IReadOnlyList<Evaluation> GetByStudent(Guid studentId);
}