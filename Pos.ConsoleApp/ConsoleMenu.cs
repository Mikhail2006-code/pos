using System.Globalization;
using Pos.Application;
using Pos.Domain;

namespace Pos.ConsoleApp;

public class ConsoleMenu
{
    private readonly PosFacade _facade;

    public ConsoleMenu(PosFacade facade)
    {
        _facade = facade;
    }

    public void Run()
    {
        while (true)
        {
            PrintMenu();
            var choice = Console.ReadLine();
            if (choice is null)
                return;

            Console.WriteLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        AddStudent();
                        break;
                    case "2":
                        ShowStudents();
                        break;
                    case "3":
                        AddCriterion();
                        break;
                    case "4":
                        ShowRubric();
                        break;
                    case "5":
                        SetEvaluation();
                        break;
                    case "6":
                        ShowEvaluations();
                        break;
                    case "7":
                        CancelEvaluation();
                        break;
                    case "8":
                        ShowFinalGrade();
                        break;
                    case "9":
                        ExportFinalGrades();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Нет такого пункта меню.");
                        break;
                }
            }
            catch (DomainException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.WriteLine();
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine("===== ПОС =====");
        Console.WriteLine("1. Добавить студента");
        Console.WriteLine("2. Список студентов");
        Console.WriteLine("3. Добавить критерий");
        Console.WriteLine("4. Рубрикатор");
        Console.WriteLine("5. Выставить или исправить оценку");
        Console.WriteLine("6. Оценки студента");
        Console.WriteLine("7. Отменить оценку");
        Console.WriteLine("8. Итог студента");
        Console.WriteLine("9. Экспорт итогов в CSV");
        Console.WriteLine("0. Выход");
        Console.Write("Выберите пункт: ");
    }

    private void AddStudent()
    {
        var name = ReadText("Имя студента: ");
        var student = _facade.AddStudent(name);
        Console.WriteLine($"Студент «{student.Name}» добавлен.");
    }

    private void ShowStudents()
    {
        var students = _facade.GetStudents();
        if (students.Count == 0)
        {
            Console.WriteLine("Студентов пока нет.");
            return;
        }

        for (var i = 0; i < students.Count; i++)
            Console.WriteLine($"{i + 1}. {students[i].Name}");
    }

    private void AddCriterion()
    {
        var name = ReadText("Название критерия: ");
        var maxScore = ReadDouble("Максимальный балл: ");
        var weight = ReadDouble("Вес: ");
        var criterion = _facade.AddCriterion(name, maxScore, weight);
        Console.WriteLine($"Критерий «{criterion.Name}» добавлен в рубрикатор.");
    }

    private void ShowRubric()
    {
        var criteria = _facade.GetCriteria();
        if (criteria.Count == 0)
        {
            Console.WriteLine("Рубрикатор пока пуст.");
            return;
        }

        for (var i = 0; i < criteria.Count; i++)
            Console.WriteLine($"{i + 1}. {criteria[i].Name}: максимум {criteria[i].MaxScore}, вес {criteria[i].Weight}");
    }

    private void SetEvaluation()
    {
        var student = ChooseStudent();
        if (student is null)
            return;

        var criterion = ChooseCriterion();
        if (criterion is null)
            return;

        var value = ReadDouble($"Балл (0–{criterion.MaxScore}): ");
        var comment = ReadText("Комментарий (можно оставить пустым): ");

        _facade.SetEvaluation(student.Id, criterion.Id, value, comment.Length == 0 ? null : comment);
        Console.WriteLine($"Оценка выставлена: {student.Name}, «{criterion.Name}» — {value}.");
    }

    private void ShowEvaluations()
    {
        var student = ChooseStudent();
        if (student is null)
            return;

        var evaluations = _facade.GetEvaluations(student.Id);
        if (evaluations.Count == 0)
        {
            Console.WriteLine($"У студента «{student.Name}» пока нет оценок.");
            return;
        }

        var criteria = _facade.GetCriteria();
        Console.WriteLine($"Оценки студента «{student.Name}»:");
        foreach (var evaluation in evaluations)
        {
            var criterion = FindCriterion(criteria, evaluation.CriterionId);
            var line = $"  {criterion.Name}: {evaluation.Score.Value} из {criterion.MaxScore} ({evaluation.Date:dd.MM.yyyy HH:mm})";
            if (evaluation.Comment is not null)
                line += $" — {evaluation.Comment}";

            Console.WriteLine(line);
        }
    }

    private void CancelEvaluation()
    {
        var student = ChooseStudent();
        if (student is null)
            return;

        var criterion = ChooseCriterion();
        if (criterion is null)
            return;

        _facade.CancelEvaluation(student.Id, criterion.Id);
        Console.WriteLine($"Оценка студента «{student.Name}» по критерию «{criterion.Name}» отменена.");
    }

    private void ShowFinalGrade()
    {
        var student = ChooseStudent();
        if (student is null)
            return;

        var grade = _facade.GetFinalGrade(student.Id);
        Console.WriteLine($"Итог студента «{student.Name}»: {grade} из 10 (правило: {_facade.GradingRuleName}).");
    }

    private void ExportFinalGrades()
    {
        var path = _facade.ExportFinalGrades();
        Console.WriteLine($"Итоги выгружены в файл: {path}");
    }

    private Student? ChooseStudent()
    {
        var students = _facade.GetStudents();
        if (students.Count == 0)
        {
            Console.WriteLine("Сначала добавьте студента.");
            return null;
        }

        for (var i = 0; i < students.Count; i++)
            Console.WriteLine($"{i + 1}. {students[i].Name}");

        return students[ReadNumber("Номер студента: ", students.Count) - 1];
    }

    private Criterion? ChooseCriterion()
    {
        var criteria = _facade.GetCriteria();
        if (criteria.Count == 0)
        {
            Console.WriteLine("Сначала добавьте критерий в рубрикатор.");
            return null;
        }

        for (var i = 0; i < criteria.Count; i++)
            Console.WriteLine($"{i + 1}. {criteria[i].Name} (максимум {criteria[i].MaxScore})");

        return criteria[ReadNumber("Номер критерия: ", criteria.Count) - 1];
    }

    private static Criterion FindCriterion(IReadOnlyList<Criterion> criteria, Guid criterionId)
    {
        foreach (var criterion in criteria)
        {
            if (criterion.Id == criterionId)
                return criterion;
        }

        throw new DomainException("Критерий оценки не найден в рубрикаторе.");
    }

    private static int ReadNumber(string prompt, int max)
    {
        while (true)
        {
            var number = ReadInt(prompt);
            if (number >= 1 && number <= max)
                return number;

            Console.WriteLine($"Нужно число от 1 до {max}.");
        }
    }

    private static string ReadText(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? "";
    }

    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out var value))
                return value;

            Console.WriteLine("Нужно ввести целое число.");
        }
    }

    private static double ReadDouble(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Replace(',', '.');
            if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
                return value;

            Console.WriteLine("Нужно ввести число, например 1.5");
        }
    }
}