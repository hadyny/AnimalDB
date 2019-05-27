using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IDocumentService
    {
        Task<IEnumerable<Document>> GetDocuments();

        Task CreateDocument(Document document);

        Task<Document> GetDocumentById(int id);

        Task UpdateDocument(Document document);

        Task DeleteDocument(Document document);

        Task<IEnumerable<Document>> GetDocumentsByCategoryId(int? categoryId);

        Task<bool> DoesDocumentFileNameExist(string fileName);
    }
}
