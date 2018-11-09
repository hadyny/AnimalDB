using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimalDB.Repo.Entities
{
    public class CleaningTask
    {
        public int id { get; set; }

        [Display(Name="Location")]
        public int CleaningLocation_Id { get; set; }
        [ForeignKey("CleaningLocation_Id")]
        public virtual CageLocation Location { get; set; }

        [Display(Name = "Room")]
        public int CleaningRoomLocation_Id { get; set; }
        [ForeignKey("CleaningRoomLocation_Id")]
        public virtual Room Room { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [Display(Name = "Date Completed")]
        public DateTime? DateCompleted { get; set; }
    }
}