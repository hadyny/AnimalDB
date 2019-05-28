using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimalDB.Repo.Entities
{
    public class Document
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }

        [Display(Name = "Date Uploaded")]
        public DateTime DateUploaded { get; set; }

        [Display(Name = "Category")]
        public int Category_Id { get; set; }
        [ForeignKey("Category_Id")]
        public virtual DocumentCategory Category { get; set; }
    }
}
