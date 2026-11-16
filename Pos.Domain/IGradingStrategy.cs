namespace Pos.Domain;

public interface IGradingStrategy
{
    string Name { get; }
    double Combine(IReadOnlyList<CriterionResult> results);
}