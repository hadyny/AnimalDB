using AnimalDBCore.Infrastructure.Contexts;
using AnimalDBCore.Core.Entities;
using AnimalDBCore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDBCore.Infrastructure.Data
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
