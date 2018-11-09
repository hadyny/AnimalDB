using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimalDB.Repo.Entities
{
    public class NotCheckedRoom
    {
        public int Id { get; set; }

        [Display(Name = "Room")]
        public int Room_Id { get; set; }

        [ForeignKey("Room_Id")]
        public virtual Room Room { get; set; }

        public DateTime Timestamp { get; set; }
    }
}