using Pos.Domain;

namespace Pos.Tests;

public class DomainTests
{
    [Fact]
    public void Score_Negative_Throws()
    {
        Assert.Throws<DomainException>(() => new Score(-1));
    }

    [Fact]
    public void Criterion_NonPositiveMaxScore_Throws()
    {
        Assert.Throws<DomainException>(() => new Criterion("Код", 0, 1));
    }

    [Fact]
    public void Rubric_DuplicateCriterionName_Throws()
    {
        var rubric = new Rubric();
        rubric.AddCriterion(new Criterion("Код", 10, 1));

        Assert.Throws<DomainException>(() => rubric.AddCriterion(new Criterion("код", 5, 1)));
    }

    [Fact]
    public void Factory_ScoreAboveMax_Throws()
    {
        var rubric = new Rubric();
        var criterion = new Criterion("Код", 10, 1);
        rubric.AddCriterion(criterion);
        var factory = new EvaluationFactory();

        Assert.Throws<DomainException>(() => factory.Create(new Student("Иван"), rubric, criterion.Id, 11, null));
    }

    [Fact]
    public void Factory_CriterionNotInRubric_Throws()
    {
        var rubric = new Rubric();
        var foreignCriterion = new Criterion("Чужой", 10, 1);
        var factory = new EvaluationFactory();

        Assert.Throws<DomainException>(() => factory.Create(new Student("Иван"), rubric, foreignCriterion.Id, 5, null));
    }
}