using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;

namespace AnimalDB.Repo.Implementations
{
    public class DocumentCategoryRepo : IDocumentCategory
    {
        private readonly AnimalDBContext db;

        public DocumentCategoryRepo()
        {
            this.db = new AnimalDBContext();
        }

        public DocumentCategoryRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateDocumentCategory(DocumentCategory documentCategory)
        {
            db.DocumentCategories.Add(documentCategory);
            await db.SaveChangesAsync();
        }

        public async Task DeleteDocumentCategory(DocumentCategory documentCategory)
        {
            db.DocumentCategories.Remove(documentCategory);
            await db.SaveChangesAsync();
        }

        public IEnumerable<DocumentCategory> GetDocumentCategories()
        {
            return db.DocumentCategories.ToList();
        }

        public async Task<DocumentCategory> GetDocumentCategoryById(int id)
        {
            return await db.DocumentCategories.FindAsync(id);
        }

        public async Task UpdateDocumentCategory(DocumentCategory documentCategory)
        {
            db.Entry(documentCategory).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
