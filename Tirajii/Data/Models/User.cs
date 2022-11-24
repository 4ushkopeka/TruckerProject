using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tirajii.Data.Models
{
    public class User:IdentityUser
    {
        public bool IsTrucker { get; set; }

        public bool IsTruckerCompanyOwner { get; set; }

        public bool IsOfferCompanyOwner { get; set; }

        public Trucker? Trucker { get; set; }

        public Company? Company { get; set; }

        public Wallet? Wallet { get; set; }

        public bool HasWallet { get; set; }
    }
}
