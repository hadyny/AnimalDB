using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimalDB.Repo.Entities
{
    public class CageLocation
    {
        public CageLocation()
        {
            this.CageLocationHistories = new HashSet<CageLocationHistory>();
        }

        public int Id { get; set; }
        [Required, Display(Name = "Cage Location")]
        public string Description { get; set; }

        [Required, Display(Name="Room")]
        public int? Room_Id { get; set; }
        [ForeignKey("Room_Id")]
        public virtual Room Room { get; set; }

        public virtual ICollection<CageLocationHistory> CageLocationHistories { get; set; }
    }
}