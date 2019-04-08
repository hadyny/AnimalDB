using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalDBCore.Core.Interfaces
{
    public interface IAnimalUser
    {
        string UserName { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }
    }
}
