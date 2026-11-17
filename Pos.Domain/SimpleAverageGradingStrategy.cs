namespace Pos.Domain;

public class SimpleAverageGradingStrategy : IGradingStrategy
{
    public string Name => "Простое среднее";

    public double Combine(IReadOnlyList<CriterionResult> results)
    {
        double sum = 0;
        foreach (var result in results)
            sum += result.Fraction;

        return sum / results.Count;
    }
}