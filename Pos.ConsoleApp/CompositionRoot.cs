using Microsoft.Extensions.DependencyInjection;
using Pos.Application;
using Pos.Domain;
using Pos.Infrastructure;

namespace Pos.ConsoleApp;

public static class CompositionRoot
{
    public static ServiceProvider Build()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IStudentRepository, InMemoryStudentRepository>();
        services.AddSingleton<IRubricRepository, InMemoryRubricRepository>();
        services.AddSingleton<IEvaluationRepository, InMemoryEvaluationRepository>();
        services.AddSingleton<IGradeExporter>(new CsvGradeExporter("final-grades.csv"));

        services.AddSingleton<EvaluationFactory>();
        services.AddSingleton<IGradingStrategy, WeightedGradingStrategy>();
        services.AddSingleton<FinalGradeCalculator>();

        services.AddSingleton<PosFacade>();
        services.AddSingleton<ConsoleMenu>();

        return services.BuildServiceProvider();
    }
}