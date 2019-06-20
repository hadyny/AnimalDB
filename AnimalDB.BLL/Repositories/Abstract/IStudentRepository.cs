using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface IStudentRepository : IUserRepository<Student>
    {
        IEnumerable<AnimalUser> GetStudentsAndInvestigators();
        Task AddInvestigatorToStudent(string studentId, string investigatorId);
        Task RemoveInvestigatorFromStudent(string studentId, string investigatorId);
    }
}
