using AnimalDBCore.Infrastructure.Contexts;
using  AnimalDBCore.Core.Entities;
using  AnimalDBCore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDBCore.Infrastructure.Data
{
    public class AnimalRoomCountRepo : IAnimalRoomCount, IDisposable
    {
        private AnimalDBContext db;

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

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
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
