using System.Collections.Generic;
using System.Threading.Tasks;
using AnimalDB.Repo.Entities;

namespace AnimalDB.Repo.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetStudents();

        Task<IEnumerable<AnimalUser>> GetStudentsAndInvestigators();

        Task<Student> GetStudentByUsername(string username);

        Task CreateStudent(Student student);

        Task<Student> GetStudentById(string id);

        Task UpdateStudent(Student student);

        Task DeleteStudent(Student student);

        Task SetAuthCookie(string userName);

        Task AddInvestigatorToStudent(string studentId, string investigatorId);

        Task RemoveInvestigatorFromStudent(string studentId, string investigatorId);
    }
}
