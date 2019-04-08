namespace  AnimalDBCore.Core.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CageLocationHistory
    {        
        public int Id { get; set; }
        [Display(Name = "Date Moved to Location")]
        public DateTime Timestamp { get; set; }
        [Display(Name = "Cage Location")]
        public int? CageLocation_Id { get; set; }
        [ForeignKey("CageLocation_Id")]
        public virtual CageLocation CageLocation { get; set; }
        [Display(Name = "Rack Location")]
        public int? RackEntry_Id { get; set; }
        [ForeignKey("RackEntry_Id")]
        public virtual RackEntry RackEntry { get; set; }

        [Display(Name = "Animal")]
        public int Animal_Id { get; set; }
        [ForeignKey("Animal_Id")]
        public virtual Animal Animal { get; set; }
    }
}
