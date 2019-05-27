using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimalDB.Repo.Entities
{
    public class DocumentCategory
    {
        public DocumentCategory()
        {
            Documents = new HashSet<Document>();
            SubCatergories = new HashSet<DocumentCategory>();
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Document> Documents { get; set; }

        [Display(Name = "Sub-Category of")]
        public int? ParentCategory_Id { get; set; }
        [ForeignKey("ParentCategory_Id")]
        public virtual DocumentCategory ParentCategory { get; set; }
        public virtual ICollection<DocumentCategory> SubCatergories { get; set; }
    }
}
