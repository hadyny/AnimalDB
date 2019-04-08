using System.Collections.Generic;

namespace  AnimalDBCore.Core.Entities
{
    public class DocumentCategory
    {
        public DocumentCategory()
        {
            this.Documents = new List<Document>();
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}
