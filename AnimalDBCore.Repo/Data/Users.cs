using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AnimalDBCore.Core.Entities;
using AnimalDBCore.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AnimalDBCore.Infrastructure.Data
{
    public class AnimalUser : IdentityUser, IAnimalUser
    {
        /*
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AnimalUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }*/

        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Full Name"), NotMapped]
        
        public string FullName
        {
            get { return this.FirstName + " " + this.LastName; }
            set { }
        }
    }

    public class Administrator : AnimalUser { }

    public class Veterinarian : AnimalUser { }

    public class Technician : AnimalUser
    {
        public virtual IEnumerable<Room> Room { get; set; }
    }

    public class Investigator : AnimalUser
    {
        public virtual ICollection<Animal> Animals { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<FeedingGroup> FeedingGroups { get; set; }
    }

    public class Student : AnimalUser
    {
        [Display(Name = "Investigator(s)")]
        public virtual ICollection<Investigator> Investigators { get; set; }
    }
}
