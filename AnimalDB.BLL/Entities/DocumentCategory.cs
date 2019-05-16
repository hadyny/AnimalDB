using System.Collections.Generic;

namespace AnimalDB.Repo.Entities
{
    public class DocumentCategory
    {
        public DocumentCategory()
        {
            Documents = new List<Document>();
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}
