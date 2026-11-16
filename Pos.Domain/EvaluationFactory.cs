namespace Pos.Domain;

public class EvaluationFactory
{
    public Evaluation Create(Student student, Rubric rubric, Guid criterionId, double value, string? comment)
    {
        var criterion = rubric.GetCriterion(criterionId);
        var score = new Score(value);

        if (!criterion.Allows(score))
            throw new DomainException($"Балл {value} больше максимума {criterion.MaxScore} по критерию «{criterion.Name}».");

        return new Evaluation(student.Id, criterion.Id, score, comment);
    }
}