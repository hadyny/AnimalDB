using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        public RoomRepository(AnimalDBContext context) : base(context)
        {
        }

        public async Task<int> GetCountOfLivingAnimalsByRoomId(int roomId)
        {
            var room = await Context.Rooms.FindAsync(roomId);
            return room
                    .Animals
                    .Count(m => m.DeathDate == null);
        }

        public async Task<int> GetCountOfGMOAnimalsByRoomId(int roomId)
        {
            var room = await Context.Rooms.FindAsync(roomId);
            return room
                    .Animals
                    .Where(m => m.ApprovalNumber_Id != null && m.DeathDate == null)
                    .Count();
        }

        public async Task<IEnumerable<Animal>> GetLivingAnimalsByRoomId(int roomId)
        {
            var room = await Context.Rooms.FindAsync(roomId);
            return room.Animals.Where(m => m.DeathDate == null).ToList();
        }


        public IEnumerable<Room> NotCheckedToday()
        {
            return Context.Rooms.Where(m =>
                            m.NoDBAnimals &&
                            (!m.NoDBAnimalsLastCheck.HasValue ||
                                (m.NoDBAnimalsLastCheck.HasValue &&
                                DbFunctions.TruncateTime(m.NoDBAnimalsLastCheck.Value) != DbFunctions.TruncateTime(DateTime.Now))))
                .ToList();

        }

        public async Task MarkAllAnimalsAsCheckedByRoomId(int roomId)
        {
            var room = await Context.Rooms.FindAsync(roomId);

            foreach (var animal in room.Animals.Where(m => m.DeathDate == null))
            {
                animal.LastChecked = DateTime.Now.Date;
            }
        }

        public async Task MarkRoomAsChecked(int roomId)
        {
            var room = await Context.Rooms.FindAsync(roomId);

            room.NoDBAnimalsLastCheck = DateTime.Now;
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
