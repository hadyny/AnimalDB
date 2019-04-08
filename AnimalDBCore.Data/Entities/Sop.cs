using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  AnimalDBCore.Core.Entities
{
    public class Sop
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }
        
        public byte[] Content { get; set; }

        [Display(Name = "Date Uploaded")]
        public DateTime DateUploaded { get; set; }

        [Display(Name = "Category")]
        public int Category_Id { get; set; }
        [ForeignKey("Category_Id")]
        public virtual SopCategory Category { get; set; }
    }

    public class SopCategory
    {
        public SopCategory() {
            this.Sops = new List<Sop>();
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Sop> Sops { get; set; }
    }


}
