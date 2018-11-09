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
    public class TransgeneRepo : ITransgene, IDisposable
    {
        private AnimalDBContext db;

        public TransgeneRepo()
        {
            this.db = new AnimalDBContext();
        }
        public TransgeneRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateTransgene(Transgene Transgene)
        {
            db.Transgenes.Add(Transgene);
            await db.SaveChangesAsync();
        }

        public async Task DeleteTransgene(Transgene Transgene)
        {
            if (db.Entry(Transgene).State == EntityState.Detached)
            {
                db.Transgenes.Attach(Transgene);
            }
            db.Transgenes.Remove(Transgene);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<Transgene> GetTransgeneById(int id)
        {
            return await db.Transgenes.FindAsync(id);
        }

        public IEnumerable<Transgene> GetTransgenes()
        {
            return db.Transgenes.ToList();
        }

        public async Task UpdateTransgene(Transgene Transgene)
        {
            db.Entry(Transgene).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
