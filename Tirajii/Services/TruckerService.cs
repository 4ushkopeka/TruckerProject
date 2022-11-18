using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tirajii.Data;
using Tirajii.Data.Models;
using Tirajii.Models.Trucker;
using Tirajii.Services.Contracts;

namespace Tirajii.Services
{
    public class TruckerService : ITruckerService
    {
        private readonly TruckersDbContext context;

        public TruckerService(TruckersDbContext context)
        {
            this.context = context;
        }
        public async Task RegisterTrucker(TruckerRegisterViewModel model, string userId)
        {
            Trucker trucker = new Trucker()
            {
                Name = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserId = userId,
                BirthDate = model.BirthDate,
                ProfilePicture = model.ProfilePicture,
            };
            await context.Truckers.AddAsync(trucker);
            await context.SaveChangesAsync();
        }
    }
}
