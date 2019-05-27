using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class SourceService : ISourceService
    {
        private readonly IRepository<Source> _sources;

        public SourceService(IRepository<Source> sources)
        {
            this._sources = sources;
        }

        public async Task CreateSource(Source source)
        {
            _sources.Insert(source);
            await _sources.Save();
        }

        public async Task DeleteSource(Source source)
        {
            await _sources.Delete(source.Id);
            await _sources.Save();
        }

        public async Task<Source> GetSourceById(int id)
        {
            return await _sources.GetById(id);
        }

        public async Task<IEnumerable<Source>> GetSources()
        {
            return await _sources.GetAll();
        }

        public async Task UpdateSource(Source source)
        {
            _sources.Update(source);
            await _sources.Save();
        }
    }
}
