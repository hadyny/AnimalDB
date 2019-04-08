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
    public class DocumentRepo : IDocument
    {
        private AnimalDBContext db;

        public DocumentRepo()
        {
            this.db = new AnimalDBContext();
        }

        public DocumentRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateDocument(Document document)
        {
            db.Documents.Add(document);
            await db.SaveChangesAsync();
        }

        public async Task DeleteDocument(Document document)
        {
            db.Documents.Remove(document);
            await db.SaveChangesAsync();
        }

        public bool DoesDocumentFileNameExist(string fileName)
        {
            return db.Documents.Count(m => m.FileName == fileName) != 0;
        }

        public async Task<Document> GetDocumentById(int id)
        {
            return await db.Documents.FindAsync(id);
        }

        public IEnumerable<Document> GetDocuments()
        {
            return db.Documents.ToList();
        }

        public IEnumerable<Document> GetDocumentsByCategoryId(int categoryId)
        {
            return GetDocuments().Where(m => m.Category_Id == categoryId);
        }

        public async Task UpdateDocument(Document document)
        {
            db.Entry(document).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }        
    }
}
