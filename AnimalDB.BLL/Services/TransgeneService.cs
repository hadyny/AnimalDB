using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class TransgeneService : ITransgeneService
    {
        private readonly IRepository<Transgene> _transgenes;

        public TransgeneService(IRepository<Transgene> transgenes)
        {
            this._transgenes = transgenes;
        }

        public async Task CreateTransgene(Transgene transgene)
        {
            _transgenes.Insert(transgene);
            await _transgenes.Save();
        }

        public async Task DeleteTransgene(Transgene transgene)
        {
            await _transgenes.Delete(transgene.Id);
            await _transgenes.Save();
        }

        public async Task<Transgene> GetTransgeneById(int id)
        {
            return await _transgenes.GetById(id);
        }

        public async Task<IEnumerable<Transgene>> GetTransgenes()
        {
            return await _transgenes.GetAll();
        }

        public async Task UpdateTransgene(Transgene transgene)
        {
            _transgenes.Update(transgene);
            await _transgenes.Save();
        }
    }
}
