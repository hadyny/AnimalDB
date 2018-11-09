using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimalDB.Repo.Entities
{
    public class Rack
    {
        public int Id { get; set; }

        [Display(Name = "Rack Identifier")]
        public string Reference_Id { get; set; }

        [Display(Name = "Room")]
        public int Room_Id { get; set; }
        [ForeignKey("Room_Id")]
        public virtual Room Room { get; set; }

        public int Width { get; set; }
        [Range(1, 26)]
        public int Height { get; set; }

        public virtual ICollection<RackEntry> RackEntries { get; set; }
    }

    public class RackEntry
    {
        public int Id { get; set; }

        [Display(Name = "Rack")]
        public int Rack_Id { get; set; }
        [ForeignKey("Rack_Id")]
        public virtual Rack Rack { get; set; }

        public int LocationReferenceX { get; set; }

        public string LocationReferenceY { get; set; }

        [Display(Name = "Animal")]
        public int? Animal_Id { get; set; }
        [ForeignKey("Animal_Id")]
        public virtual Animal Animal { get; set; }

        public bool IsCurrent { get; set; }

        [NotMapped]
        public string Reference
        {
            get
            {
                return this.LocationReferenceY + this.LocationReferenceX.ToString();
            }
        }
    }
}
