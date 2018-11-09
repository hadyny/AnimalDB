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
    public class StrainRepo : IStrain, IDisposable
    {
        private AnimalDBContext db;

        public StrainRepo()
        {
            this.db = new AnimalDBContext();
        }

        public StrainRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateStrain(Strain Strain)
        {
            db.Strains.Add(Strain);
            await db.SaveChangesAsync();
        }

        public async Task DeleteStrain(Strain Strain)
        {
            if (db.Entry(Strain).State == EntityState.Detached)
            {
                db.Strains.Attach(Strain);
            }
            db.Strains.Remove(Strain);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<Strain> GetStrainById(int id)
        {
            return await db.Strains.FindAsync(id);
        }

        public IEnumerable<Strain> GetStrains()
        {
            return db.Strains.ToList();
        }

        public async Task UpdateStrain(Strain Strain)
        {
            db.Entry(Strain).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
