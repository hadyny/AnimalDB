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
    public class SopCategoryRepo : ISopCategory, IDisposable
    {
        private AnimalDBContext db;

        public SopCategoryRepo()
        {
            this.db = new AnimalDBContext();
        }

        public SopCategoryRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateSopCategory(SopCategory sopCategory)
        {
            db.SopCategories.Add(sopCategory);
            await db.SaveChangesAsync();
        }

        public async Task DeleteSopCategory(SopCategory sopCategory)
        {
            if (db.Entry(sopCategory).State == EntityState.Detached)
            {
                db.SopCategories.Attach(sopCategory);
            }
            db.SopCategories.Remove(sopCategory);
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }

        public async Task<SopCategory> GetSopCategoryById(int id)
        {
            return await db.SopCategories.FindAsync(id);
        }

        public IEnumerable<SopCategory> GetSopCategories()
        {
            return db.SopCategories.ToList();
        }

        public async Task UpdateSopCategory(SopCategory sopCategory)
        {
            db.Entry(sopCategory).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
