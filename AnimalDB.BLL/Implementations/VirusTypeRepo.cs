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
    public class VirusTypeRepo : IVirusType, IDisposable
    {
        private AnimalDBContext db;

        public VirusTypeRepo()
        {
            this.db = new AnimalDBContext();
        }

        public VirusTypeRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateVirusType(VirusType virusType)
        {
            db.VirusTypes.Add(virusType);
            await db.SaveChangesAsync();
        }

        public async Task DeleteVirusType(VirusType virusType)
        {
            if (db.Entry(virusType).State == EntityState.Detached)
            {
                db.VirusTypes.Attach(virusType);
            }
            db.VirusTypes.Remove(virusType);
            await db.SaveChangesAsync();
        }

        public async Task<VirusType> GetVirusTypeById(int id)
        {
            return await db.VirusTypes.FindAsync(id);
        }

        public IEnumerable<VirusType> GetVirusTypes()
        {
            return db.VirusTypes.ToList();
        }

        public async Task UpdateVirusType(VirusType VirusType)
        {
            db.Entry(VirusType).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}
