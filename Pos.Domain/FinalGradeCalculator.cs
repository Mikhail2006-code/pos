namespace Pos.Domain;

public class FinalGradeCalculator
{
    private readonly IGradingStrategy _strategy;

    public FinalGradeCalculator(IGradingStrategy strategy)
    {
        _strategy = strategy;
    }

    public string StrategyName => _strategy.Name;

    public double Calculate(Rubric rubric, IReadOnlyList<Evaluation> evaluations)
    {
        if (rubric.Criteria.Count == 0)
            throw new DomainException("Рубрикатор пуст, итог посчитать нельзя.");

        var results = new List<CriterionResult>();
        foreach (var criterion in rubric.Criteria)
        {
            double score = 0;
            foreach (var evaluation in evaluations)
            {
                if (evaluation.CriterionId == criterion.Id)
                    score = evaluation.Score.Value;
            }

            results.Add(new CriterionResult(criterion, score / criterion.MaxScore));
        }

        return Math.Round(_strategy.Combine(results) * 10, 2);
    }
}