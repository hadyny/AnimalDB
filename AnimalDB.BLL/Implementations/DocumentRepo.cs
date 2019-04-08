using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Implementations
{
    public class DocumentRepo : IDocument
    {
        private readonly AnimalDBContext db;

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
