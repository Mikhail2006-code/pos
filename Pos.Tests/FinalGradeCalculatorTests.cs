using Pos.Domain;

namespace Pos.Tests;

public class FinalGradeCalculatorTests
{
    private readonly Rubric _rubric = new();
    private readonly Criterion _test = new("Контрольная", 10, 2);
    private readonly Criterion _homework = new("Домашка", 5, 1);
    private readonly Student _student = new("Иван");
    private readonly EvaluationFactory _factory = new();

    public FinalGradeCalculatorTests()
    {
        _rubric.AddCriterion(_test);
        _rubric.AddCriterion(_homework);
    }

    [Fact]
    public void Calculate_WeightedStrategy_UsesWeights()
    {
        var calculator = new FinalGradeCalculator(new WeightedGradingStrategy());
        var evaluations = new List<Evaluation>
        {
            _factory.Create(_student, _rubric, _test.Id, 8, null),
            _factory.Create(_student, _rubric, _homework.Id, 5, null)
        };

        Assert.Equal(8.67, calculator.Calculate(_rubric, evaluations), 2);
    }

    [Fact]
    public void Calculate_SimpleAverageStrategy_IgnoresWeights()
    {
        var calculator = new FinalGradeCalculator(new SimpleAverageGradingStrategy());
        var evaluations = new List<Evaluation>
        {
            _factory.Create(_student, _rubric, _test.Id, 8, null),
            _factory.Create(_student, _rubric, _homework.Id, 5, null)
        };

        Assert.Equal(9.0, calculator.Calculate(_rubric, evaluations), 2);
    }

    [Fact]
    public void Calculate_MissingEvaluation_CountsAsZero()
    {
        var calculator = new FinalGradeCalculator(new WeightedGradingStrategy());
        var evaluations = new List<Evaluation>
        {
            _factory.Create(_student, _rubric, _test.Id, 8, null)
        };

        Assert.Equal(5.33, calculator.Calculate(_rubric, evaluations), 2);
    }

    [Fact]
    public void Calculate_EmptyRubric_Throws()
    {
        var calculator = new FinalGradeCalculator(new WeightedGradingStrategy());

        Assert.Throws<DomainException>(() => calculator.Calculate(new Rubric(), new List<Evaluation>()));
    }
}