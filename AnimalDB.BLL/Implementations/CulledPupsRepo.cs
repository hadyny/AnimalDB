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
    public class CulledPupsRepo : ICulledPups
    {
        private readonly AnimalDBContext db;

        public CulledPupsRepo()
        {
            this.db = new AnimalDBContext();
        }

        public CulledPupsRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateCulledPups(CulledPups culledPups)
        {
            db.CulledPups.Add(culledPups);
            await db.SaveChangesAsync();
        }

        public async Task DeleteCulledPups(CulledPups culledPups)
        {
            if (db.Entry(culledPups).State == EntityState.Detached)
            {
                db.CulledPups.Attach(culledPups);
            }
            db.CulledPups.Remove(culledPups);
            await db.SaveChangesAsync();
        }

        public async Task<CulledPups> GetCulledPupsById(int id)
        {
            return await db.CulledPups.FindAsync(id);
        }

        public IEnumerable<CulledPups> GetCulledPups()
        {
            return db.CulledPups.ToList();
        }

        public async Task UpdateCulledPups(CulledPups culledPups)
        {
            db.Entry(culledPups).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public IEnumerable<CulledPups> GetCulledPupsByAnimalId(int animalId)
        {
            return db.CulledPups.Where(m => m.AnimalId == animalId).ToList();
        }
    }
}
