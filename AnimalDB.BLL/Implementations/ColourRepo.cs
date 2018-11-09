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
    public class ColourRepo : IColour, IDisposable
    {
        private AnimalDBContext db;

        public ColourRepo()
        {
            this.db = new AnimalDBContext();
        }
        public ColourRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateColour(Colour colour)
        {
            db.Colours.Add(colour);
            await db.SaveChangesAsync();
        }

        public async Task DeleteColour(Colour colour)
        {
            if (db.Entry(colour).State == EntityState.Detached)
            {
                db.Colours.Attach(colour);
            }
            db.Colours.Remove(colour);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<Colour> GetColourById(int id)
        {
            return await db.Colours.FindAsync(id);
        }

        public IEnumerable<Colour> GetColours()
        {
            return db.Colours.ToList();
        }

        public async Task UpdateColour(Colour colour)
        {
            db.Entry(colour).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
