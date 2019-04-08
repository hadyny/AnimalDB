using System.ComponentModel.DataAnnotations;

namespace  AnimalDBCore.Core.Entities
{
    public class ApprovalNumber
    {
        public int Id { get; set; }

        [Display(Name = "HSNO Act APP#")]
        public string Description { get; set; }

        [Display(Name = "Description")]
        public string FriendlyName { get; set; }
    }
}