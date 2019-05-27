using AnimalDB.Repo.Entities;
using System.Collections.Generic;

namespace AnimalDB.Web.Models
{
    public class DocumentCategoryViewModel
    {
        public string CategoryName { get; set; }

        public int? Category_Id { get; set; }

        public int? Parent_Id { get; set; }

        public bool IsRootCategory { get; set; }

        public bool AuthenticatedUser { get; set; }

        public IEnumerable<DocumentCategory> SubCategories { get; set; }

        public IEnumerable<Document> Documents { get; set; }

        public IEnumerable<DocumentCategory> ParentHierarchy { get; set; }
    }
}