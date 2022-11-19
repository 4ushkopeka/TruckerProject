using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Globalization;
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

        public async Task<List<TruckingCategory>> GetAllCategories()
        {
            return await context.TruckingCategories.ToListAsync();
        }

        public async Task RegisterTrucker(TruckerRegisterViewModel model, string userId)
        {
            var user = context.Users.First(x => x.Id == userId);
            bool bday = DateTime.TryParseExact(model.BirthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime BDayval);
            if (!bday)
            {
                throw new ArgumentException("Invalid date format!");
            }
            Trucker trucker = new Trucker()
            {
                Name = model.FullName,
                Email = model.Email,
                User = user,
                PhoneNumber = model.PhoneNumber,
                BirthDate = BDayval,
                ProfilePicture = model.ProfilePicture,
                CategoryId = model.CategoryId
            };
            user.IsTrucker = true;
            user.Trucker = trucker;
            await context.Truckers.AddAsync(trucker);
            await context.SaveChangesAsync();
        }
    }
}
