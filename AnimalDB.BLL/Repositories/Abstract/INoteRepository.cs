using AnimalDB.Repo.Entities;
using System.Collections.Generic;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface INoteRepository : IRepository<Note>
    {
        IEnumerable<Note> GetByAnimalId(int animalId);
    }
}
