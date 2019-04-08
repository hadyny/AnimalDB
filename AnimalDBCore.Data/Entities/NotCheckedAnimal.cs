using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace  AnimalDBCore.Core.Entities
{
    public class NotCheckedAnimal
    {
        public int Id { get; set; }

        [Display(Name = "Animal")]
        public int Animal_Id { get; set; }

        [ForeignKey("Animal_Id")]
        public virtual Animal Animal { get; set; }

        public DateTime Timestamp { get; set; }
    }
}