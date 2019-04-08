using System.Collections.Generic;
using System.Threading.Tasks;
using  AnimalDBCore.Core.Entities;

namespace  AnimalDBCore.Core.Interfaces
{
    public interface IStudent<T>
    {
        IEnumerable<T> GetStudents();

        T GetStudentByUsername(string username);

        Task CreateStudent(T student);

        Task<T> GetStudentById(string id);

        Task UpdateStudent(T student);

        Task DeleteStudent(T student);

        Task SetAuthCookie(string userName);

        Task AddInvestigatorToStudent(string studentId, string investigatorId);

        Task RemoveInvestigatorFromStudent(string studentId, string investigatorId);
    }
}
