using Microsoft.EntityFrameworkCore;
using Tirajii.Data;
using Tirajii.Data.Models;
using Tirajii.Models;
using Tirajii.Models.Trucker;
using Tirajii.Services.Contracts;

namespace Tirajii.Services
{
    public class TruckService : ITruckService
    {
        private readonly TruckersDbContext context;
        public TruckService(TruckersDbContext context)
        {
            this.context = context;
        }

        public async Task<Truck> GetTruckByUserId(string userId)
        {
            var user = await context.Users.Include(x => x.Trucker).ThenInclude(x => x.Truck).ThenInclude(x => x.Company).FirstAsync(x => x.Id == userId);
            return user.Trucker.Truck;
        }
    }
}
