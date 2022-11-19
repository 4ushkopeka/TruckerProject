using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Tirajii.Data;
using Tirajii.Data.Models;
using Tirajii.Models.Company;
using Tirajii.Models.Trucker;
using Tirajii.Services.Contracts;

namespace Tirajii.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly TruckersDbContext context;

        public CompanyService(TruckersDbContext context)
        {
            this.context = context;
        }

        public async Task AddOffer(OfferAddViewModel model, string userId)
        {
            var user = context.Users.Include(x => x.Company).First(x => x.Id == userId);
            var companyId = user.Company.Id;
            bool date = DateTime.TryParseExact(model.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime BDayval);
            if (!date)
            {
                throw new ArgumentException("Invalid date format!");
            }
            Offer offer = new Offer() 
            { 
                DueDate = BDayval,
                CategoryId = model.CategoryId,
                Name = model.Name,
                Description = model.Description,
                Payment = model.Payment,
                CompanyId = companyId
            };
            await context.Offers.AddAsync(offer);
            await context.SaveChangesAsync();
        }

        public async Task<List<CompanyCategory>> GetAllCategories()
        {
            return await context.CompanyCategories.ToListAsync();
        }

        public async Task<List<TruckingCategory>> GetAllOfferCategories()
        {
            return await context.TruckingCategories.ToListAsync();
        }

        public async Task RegisterCompany(CompanyRegisterViewModel model, string userId)
        {
            var user = context.Users.First(x => x.Id == userId);
            Company company = new Company()
            {
                Name = model.Name,
                Owner = user,
                Picture = model.Picture,
                CategoryId = model.CategoryId,
                Rating = 0
            };
            user.IsCompanyOwner = true;
            user.Company = company;
            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();
        }
    }
}
