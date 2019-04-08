using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface ISurgicalWelfareScore
    {
        IEnumerable<SurgicalWelfareScore> GetSurgicalWelfareScores();

        Task CreateSurgicalWelfareScore(SurgicalWelfareScore surgicalWelfareScore);

        Task<SurgicalWelfareScore> GetSurgicalWelfareScoreById(int id);

        Task UpdateSurgicalWelfareScore(SurgicalWelfareScore surgicalWelfareScore);

        Task DeleteSurgicalWelfareScore(SurgicalWelfareScore surgicalWelfareScore);
    }
}