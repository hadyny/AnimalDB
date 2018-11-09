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
    public class SpeciesRepo : ISpecies, IDisposable
    {
        private AnimalDBContext db;

        public SpeciesRepo()
        {
            this.db = new AnimalDBContext();
        }

        public SpeciesRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateSpecies(Species Species)
        {
            db.Species.Add(Species);
            await db.SaveChangesAsync();
        }

        public async Task DeleteSpecies(Species Species)
        {
            if (db.Entry(Species).State == EntityState.Detached)
            {
                db.Species.Attach(Species);
            }
            db.Species.Remove(Species);
            await db.SaveChangesAsync();
        }

        public async Task<Species> GetSpeciesById(int id)
        {
            return await db.Species.FindAsync(id);
        }

        public IEnumerable<Species> GetSpecies()
        {
            return db.Species.ToList();
        }

        public async Task UpdateSpecies(Species Species)
        {
            db.Entry(Species).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}
