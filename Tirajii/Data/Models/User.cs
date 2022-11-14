using Microsoft.AspNetCore.Identity;

namespace Tirajii.Data.Models
{
    public class User:IdentityUser
    {
        public string FullName { get; set; }
        
    }
}
