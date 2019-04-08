using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimalDB.Repo.Entities
{
    public class Roster
    {
        public int Id { get; set; }

        [Display(Name = "Rostered Student")]
        [Required]
        public string Student_Id { get; set; }
        [ForeignKey("Student_Id")]
        public virtual Student Student { get; set; }
        [Display(Name = "Weekend"), Required, UIHint("Weekend")]
        public DateTime Date { get; set; }
        [Display(Name = "Room")]
        public int Room_Id { get; set; }
        [ForeignKey("Room_Id")]
        public Room Room { get; set; }

        public virtual ICollection<RosterNote> Notes { get; set; }
    }

    public class RosterNote
    {
        public int Id { get; set; }
        [DataType(DataType.MultilineText), Required]
        public string Content { get; set; }

        public int Roster_Id { get; set; }
        [ForeignKey("Roster_Id")]
        public Roster Roster { get; set; }
    }
}
