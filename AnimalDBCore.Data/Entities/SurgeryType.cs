namespace  AnimalDBCore.Core.Entities
{
    using System.ComponentModel.DataAnnotations;
    
    public class SurgeryType
    {
        public int Id { get; set; }
        [Required, Display(Name = "Surgery Type")]
        public string Description { get; set; }
    }
}
