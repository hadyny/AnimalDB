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
    public class SourceRepo : ISource, IDisposable
    {
        private AnimalDBContext db;

        public SourceRepo()
        {
            this.db = new AnimalDBContext();
        }

        public SourceRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateSource(Source source)
        {
            db.Sources.Add(source);
            await db.SaveChangesAsync();
        }

        public async Task DeleteSource(Source source)
        {
            if (db.Entry(source).State == EntityState.Detached)
            {
                db.Sources.Attach(source);
            }
            db.Sources.Remove(source);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<Source> GetSourceById(int id)
        {
            return await db.Sources.FindAsync(id);
        }

        public IEnumerable<Source> GetSources()
        {
            return db.Sources.ToList();
        }

        public async Task UpdateSource(Source source)
        {
            db.Entry(source).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
