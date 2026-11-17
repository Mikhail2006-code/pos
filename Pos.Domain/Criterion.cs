namespace Pos.Domain;

public class Criterion
{
    public Guid Id { get; }
    public string Name { get; }
    public double MaxScore { get; }
    public double Weight { get; }

    public Criterion(string name, double maxScore, double weight)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Название критерия не может быть пустым.");
        if (maxScore <= 0)
            throw new DomainException("Максимальный балл должен быть больше нуля.");
        if (weight <= 0)
            throw new DomainException("Вес критерия должен быть больше нуля.");

        Id = Guid.NewGuid();
        Name = name.Trim();
        MaxScore = maxScore;
        Weight = weight;
    }

    public bool Allows(Score score)
    {
        return score.Value <= MaxScore;
    }
}