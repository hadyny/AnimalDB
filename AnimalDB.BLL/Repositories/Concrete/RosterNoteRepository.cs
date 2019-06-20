using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class RosterNoteRepository : Repository<RosterNote>, IRosterNoteRepository
    {

        public RosterNoteRepository(AnimalDBContext context)
        {
        }

        public IEnumerable<RosterNote> GetByRosterId(int id)
        {
            return Context.RosterNotes.Where(m => m.Roster_Id == id).ToList();
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
