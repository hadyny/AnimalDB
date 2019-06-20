using AnimalDB.Repo.Entities;
using System.Collections.Generic;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface ISurgicalNoteRepository : IRepository<SurgicalNote>
    {
        IEnumerable<SurgicalNote> GetByAnimalId(int animalId);
    }
}
