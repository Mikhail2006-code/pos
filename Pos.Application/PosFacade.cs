using Pos.Domain;

namespace Pos.Application;

public class PosFacade
{
    private readonly IStudentRepository _students;
    private readonly IRubricRepository _rubrics;
    private readonly IEvaluationRepository _evaluations;
    private readonly EvaluationFactory _factory;
    private readonly FinalGradeCalculator _calculator;
    private readonly IGradeExporter _exporter;

    public PosFacade(
        IStudentRepository students,
        IRubricRepository rubrics,
        IEvaluationRepository evaluations,
        EvaluationFactory factory,
        FinalGradeCalculator calculator,
        IGradeExporter exporter)
    {
        _students = students;
        _rubrics = rubrics;
        _evaluations = evaluations;
        _factory = factory;
        _calculator = calculator;
        _exporter = exporter;
    }

    public string GradingRuleName => _calculator.StrategyName;

    public Student AddStudent(string name)
    {
        var student = new Student(name);
        _students.Add(student);
        return student;
    }

    public IReadOnlyList<Student> GetStudents()
    {
        return _students.GetAll();
    }

    public Criterion AddCriterion(string name, double maxScore, double weight)
    {
        var rubric = _rubrics.Get();
        var criterion = new Criterion(name, maxScore, weight);
        rubric.AddCriterion(criterion);
        _rubrics.Save(rubric);
        return criterion;
    }

    public IReadOnlyList<Criterion> GetCriteria()
    {
        return _rubrics.Get().Criteria;
    }

    public Evaluation SetEvaluation(Guid studentId, Guid criterionId, double value, string? comment)
    {
        var student = GetStudent(studentId);
        var evaluation = _factory.Create(student, _rubrics.Get(), criterionId, value, comment);
        _evaluations.Save(evaluation);
        return evaluation;
    }

    public void CancelEvaluation(Guid studentId, Guid criterionId)
    {
        GetStudent(studentId);
        _evaluations.Remove(studentId, criterionId);
    }

    public IReadOnlyList<Evaluation> GetEvaluations(Guid studentId)
    {
        return _evaluations.GetByStudent(GetStudent(studentId).Id);
    }

    public double GetFinalGrade(Guid studentId)
    {
        var student = GetStudent(studentId);
        return _calculator.Calculate(_rubrics.Get(), _evaluations.GetByStudent(student.Id));
    }

    public string ExportFinalGrades()
    {
        var rows = new List<FinalGradeRow>();
        foreach (var student in _students.GetAll())
            rows.Add(new FinalGradeRow(student.Name, GetFinalGrade(student.Id)));

        return _exporter.Export(rows);
    }

    private Student GetStudent(Guid studentId)
    {
        return _students.GetById(studentId)
            ?? throw new DomainException("Студент не найден.");
    }
}