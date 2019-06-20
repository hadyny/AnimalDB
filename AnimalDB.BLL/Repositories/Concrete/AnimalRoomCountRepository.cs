using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class AnimalRoomCountRepository : Repository<AnimalRoomCount>, IAnimalRoomCountRepository
    {

        public AnimalRoomCountRepository(AnimalDBContext context) : base(context)
        {
        }

        public IEnumerable<AnimalRoomCount> GetLastNRoomCountsByRoomId(int roomId, int amount = 30)
        {
            var counts = Context.AnimalRoomCounts.Where(m => m.Room_Id == roomId);
            return counts
                    .OrderByDescending(m => m.Timestamp)
                    .Take(amount)
                    .ToList();
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
