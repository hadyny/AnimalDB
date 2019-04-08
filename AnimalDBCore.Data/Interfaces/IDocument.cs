﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using   AnimalDBCore.Core.Entities;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface IDocument
    {
        IEnumerable<Document> GetDocuments();

        Task CreateDocument(Document document);

        Task<Document> GetDocumentById(int id);

        Task UpdateDocument(Document document);

        Task DeleteDocument(Document document);

        IEnumerable<Document> GetDocumentsByCategoryId(int categoryId);

        bool DoesDocumentFileNameExist(string fileName);
    }
}