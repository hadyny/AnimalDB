namespace  AnimalDBCore.Core.Entities
{
    using  AnimalDBCore.Core.Enums;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public class EthicsNumberHistory
    {
        public int Id { get; set; }
        [Display(Name = "AUP Number")]
        public int Ethics_Id { get; set; }
        [ForeignKey("Ethics_Id")]
        public virtual EthicsNumber EthicsNumber { get; set; }

        [Display(Name = "Animal")]
        public int Animal_Id { get; set; }
        [ForeignKey("Animal_Id")]
        public virtual Animal Animal { get; set; }
        [Display(Name = "Manipulation Exit Status")]
        public AliveStatus? AliveStatus { get; set; }
        [Display(Name = "Date Assigned")]
        public DateTime Timestamp { get; set; }    
    }
}
