using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class SurgicalWelfareScoreService : ISurgicalWelfareScoreService
    {
        private readonly IRepository<SurgicalWelfareScore> _surgicalWelfareScores;

        public SurgicalWelfareScoreService(IRepository<SurgicalWelfareScore> surgicalWelfareScores)
        {
            this._surgicalWelfareScores = surgicalWelfareScores;
        }

        public async Task CreateSurgicalWelfareScore(SurgicalWelfareScore surgicalWelfareScore)
        {
            _surgicalWelfareScores.Insert(surgicalWelfareScore);
            await _surgicalWelfareScores.Save();
        }

        public async Task DeleteSurgicalWelfareScore(SurgicalWelfareScore surgicalWelfareScore)
        {
            await _surgicalWelfareScores.Delete(surgicalWelfareScore.Id);
            await _surgicalWelfareScores.Save();
        }
        public async Task<SurgicalWelfareScore> GetSurgicalWelfareScoreById(int id)
        {
            return await _surgicalWelfareScores.GetById(id);
        }

        public async Task<IEnumerable<SurgicalWelfareScore>> GetSurgicalWelfareScores()
        {
            return await _surgicalWelfareScores.GetAll();
        }

        public async Task UpdateSurgicalWelfareScore(SurgicalWelfareScore surgicalWelfareScore)
        {
            _surgicalWelfareScores.Update(surgicalWelfareScore);
            await _surgicalWelfareScores.Save();
        }
    }
}
