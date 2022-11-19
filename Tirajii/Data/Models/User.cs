using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tirajii.Data.Models
{
    public class User:IdentityUser
    {
        public bool IsTrucker { get; set; }

        public bool IsCompanyOwner { get; set; }

        public Trucker? Trucker { get; set; }

        public Company? Company { get; set; }
    }
}
