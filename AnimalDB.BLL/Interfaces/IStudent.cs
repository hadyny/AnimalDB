using System.Collections.Generic;
using System.Threading.Tasks;
using AnimalDB.Repo.Entities;

namespace AnimalDB.Repo.Interfaces
{
    public interface IStudent
    {
        IEnumerable<Student> GetStudents();

        IEnumerable<AnimalUser> GetStudentsAndInvestigators();

        Student GetStudentByUsername(string username);

        Task CreateStudent(Student student);

        Task<Student> GetStudentById(string id);

        Task UpdateStudent(Student student);

        Task DeleteStudent(Student student);

        Task SetAuthCookie(string userName);

        Task AddInvestigatorToStudent(string studentId, string investigatorId);

        Task RemoveInvestigatorFromStudent(string studentId, string investigatorId);
    }
}
