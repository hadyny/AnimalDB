using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimalDBCore.Infrastructure.Contexts;
using  AnimalDBCore.Core.Entities;
using  AnimalDBCore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnimalDBCore.Infrastructure.Data
{
    public class DocumentCategoryRepo : IDocumentCategory, IDisposable
    {
        private AnimalDBContext db;

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

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
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
