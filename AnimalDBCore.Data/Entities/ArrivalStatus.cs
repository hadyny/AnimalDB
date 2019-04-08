namespace  AnimalDBCore.Core.Entities
{
    using  AnimalDBCore.Core.Enums;
    using System.ComponentModel.DataAnnotations;
    
    public class ArrivalStatus
    {
        public int Id { get; set; }
        [Required, Display(Name = "Arrival Status")]
        public string Description { get; set; }

        public ArrivalStatusType Type { get; set; }
    }
}
