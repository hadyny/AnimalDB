using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class RosterRepository : Repository<Roster>, IRosterRepository
    {
        public RosterRepository(AnimalDBContext context) : base(context)
        {
        }

        public bool ThisWeekendsRosterExists(DateTime date, int? id = null)
        {
            var rosters = Context.Rosters.Where(m => m.Date == date);
            if (id == null)
            {
                return rosters.Count() != 0;
            }
            else
            {
                return rosters.Count(m => m.Id != id) != 0;
            }
        }

        public IEnumerable<Roster> GetByRoomId(int id)
        {
            return Context.Rosters.Where(m => m.Room_Id == id).ToList();
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
