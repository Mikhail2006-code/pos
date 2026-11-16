namespace Pos.Domain;

public interface IStudentRepository
{
    void Add(Student student);
    IReadOnlyList<Student> GetAll();
    Student? GetById(Guid id);
}