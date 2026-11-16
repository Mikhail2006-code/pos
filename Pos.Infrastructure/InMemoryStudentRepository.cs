using Pos.Domain;

namespace Pos.Infrastructure;

public class InMemoryStudentRepository : IStudentRepository
{
    private readonly List<Student> _students = new();

    public void Add(Student student)
    {
        _students.Add(student);
    }

    public IReadOnlyList<Student> GetAll()
    {
        return _students;
    }

    public Student? GetById(Guid id)
    {
        foreach (var student in _students)
        {
            if (student.Id == id)
                return student;
        }

        return null;
    }
}