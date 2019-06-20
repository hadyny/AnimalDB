using AnimalDB.Repo.Entities;
using System;
using System.Collections.Generic;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface IRosterRepository : IRepository<Roster>
    {
        bool ThisWeekendsRosterExists(DateTime date, int? id = null);
        IEnumerable<Roster> GetByRoomId(int id);
    }
}
