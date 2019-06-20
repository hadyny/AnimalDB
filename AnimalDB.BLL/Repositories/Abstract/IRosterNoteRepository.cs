using AnimalDB.Repo.Entities;
using System.Collections.Generic;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface IRosterNoteRepository : IRepository<RosterNote>
    {
        IEnumerable<RosterNote> GetByRosterId(int id);
    }
}
