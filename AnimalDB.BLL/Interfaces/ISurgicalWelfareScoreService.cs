using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface ISurgicalWelfareScoreService
    {
        Task<IEnumerable<SurgicalWelfareScore>> GetSurgicalWelfareScores();

        Task CreateSurgicalWelfareScore(SurgicalWelfareScore surgicalWelfareScore);

        Task<SurgicalWelfareScore> GetSurgicalWelfareScoreById(int id);

        Task UpdateSurgicalWelfareScore(SurgicalWelfareScore surgicalWelfareScore);

        Task DeleteSurgicalWelfareScore(SurgicalWelfareScore surgicalWelfareScore);
    }
}