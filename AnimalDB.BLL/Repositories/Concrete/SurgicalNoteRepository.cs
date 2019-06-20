using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class SurgicalNoteRepository : Repository<SurgicalNote>, ISurgicalNoteRepository
    {
        public SurgicalNoteRepository(AnimalDBContext context) : base(context)
        {
        }

        public IEnumerable<SurgicalNote> GetByAnimalId(int animalId)
        {
            return Context.SurgicalNotes.Where(m => m.Animal_Id == animalId).ToList();
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
