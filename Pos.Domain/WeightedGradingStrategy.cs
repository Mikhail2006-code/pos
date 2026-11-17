namespace Pos.Domain;

public class WeightedGradingStrategy : IGradingStrategy
{
    public string Name => "Взвешенное среднее";

    public double Combine(IReadOnlyList<CriterionResult> results)
    {
        double weightedSum = 0;
        double totalWeight = 0;

        foreach (var result in results)
        {
            weightedSum += result.Fraction * result.Criterion.Weight;
            totalWeight += result.Criterion.Weight;
        }

        return weightedSum / totalWeight;
    }
}