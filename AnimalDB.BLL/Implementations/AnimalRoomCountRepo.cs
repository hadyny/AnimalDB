using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Implementations
{
    public class AnimalRoomCountRepo : IAnimalRoomCount
    {
        private readonly AnimalDBContext db;

        public AnimalRoomCountRepo()
        {
            this.db = new AnimalDBContext();
        }

        public AnimalRoomCountRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateAnimalRoomCount(AnimalRoomCount animalRoomCount)
        {
            db.AnimalRoomCounts.Add(animalRoomCount);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAnimalRoomCount(AnimalRoomCount animalRoomCount)
        {
            if (db.Entry(animalRoomCount).State == EntityState.Detached)
            {
                db.AnimalRoomCounts.Attach(animalRoomCount);
            }
            db.AnimalRoomCounts.Remove(animalRoomCount);
            await db.SaveChangesAsync();
        }

        public IEnumerable<AnimalRoomCount> GetLastNRoomCountsByRoomId(int roomId, int amount = 30)
        {
            return db.AnimalRoomCounts
                    .Where(m => m.Room_Id == roomId)
                    .OrderByDescending(m => m.Timestamp)
                    .Take(amount)
                    .ToList();
        }

        public async Task<AnimalRoomCount> GetAnimalRoomCountById(int id)
        {
            return await db.AnimalRoomCounts.FindAsync(id);
        }

        public IEnumerable<AnimalRoomCount> GetAnimalRoomCounts()
        {
            return db.AnimalRoomCounts.ToList();
        }

        public async Task UpdateAnimalRoomCount(AnimalRoomCount animalRoomCount)
        {
            db.Entry(animalRoomCount).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
