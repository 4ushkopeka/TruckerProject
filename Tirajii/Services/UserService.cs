using System.Security.Claims;
using Tirajii.Data;
using Tirajii.Data.Models;
using Tirajii.Services.Contracts;

namespace Tirajii.Services
{
    public class UserService:IUserService
    {
        private readonly TruckersDbContext context;
        public UserService(TruckersDbContext _context)
        {
            context = _context;
        }
    }
}
