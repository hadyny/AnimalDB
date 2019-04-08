namespace  AnimalDBCore.Core.Entities
{
    using  AnimalDBCore.Core.Enums;
    using AnimalDBCore.Core.Interfaces;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Note
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        [Required]
        public NoteType? Type { get; set; }
        [Required]
        public string Text { get; set; }
        [Display(Name = "Animal")]
        public int Animal_Id { get; set; }
        [ForeignKey("Animal_Id")]
        public virtual Animal Animal { get; set; }

        public string TechnicianNotified_Id { get; set; }
        [ForeignKey("TechnicianNotified_Id")]
        public virtual IAnimalUser TechnicianNotified { get; set; }
    }
}
