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
                CompanyId = companyId,
                IsApproved = true
            };
            await context.Offers.AddAsync(offer);
            await context.SaveChangesAsync();
        }

        public async Task<List<CompanyCategory>> GetAllCompanyCategories()
        {
            return await context.CompanyCategories.ToListAsync();
        }

        public async Task<List<TruckClass>> GetAllClasses()
        {
            return await context.TruckClasses.ToListAsync();
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
                CategoryId = model.CategoryId
            };
            if (model.CategoryId==1) user.IsOfferCompanyOwner = true;
            else user.IsTruckerCompanyOwner = true;
            user.Company = company;
            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();
        }

        public async Task RegisterTruck(TruckViewModel model, string userId)
        {
            var user = context.Users.Include(x => x.Company).First(x => x.Id == userId);
            Truck truck = new Truck()
            {
                Name = model.Name,
                CompanyId = user.Company.Id,
                Picture = model.Picture,
                ClassId= model.ClassId,
                Colour = model.Colour,
                IsForSale = false,
                RegistrationNumber = model.RegistrationNumber,
                HasBluetooth = model.HasBluetooth,
                HasCDPlayer = model.HasCDPlayer,
                HasInstaBrakes = model.HasInstaBrakes,
                HasParkTronic = model.HasParkTronic,
                HasSpeakers = model.HasSpeakers
            };
            await context.Trucks.AddAsync(truck);
            await context.SaveChangesAsync();
        }
        public async Task AddTruckOffer(TruckOfferAddViewModel model, string userId)
        {
            var user = context.Users.Include(x => x.Company).First(x => x.Id == userId);
            var truck = this.GetTruckById(model.truckId).Result;
            TruckOffer offer = new TruckOffer()
            {
                Company = user.Company,
                Description = model.Description,
                Cost = model.Cost,
                Truck = truck,
                Name = model.Name,
                IsApproved = true,
                CompanyId = (int)model.CompanyId
            };
            truck.IsForSale = true;
            await context.TruckOffers.AddAsync(offer);
            await context.SaveChangesAsync();
        }

        public async Task<List<Truck>> GetMyTrucks(string userId)
        {
            var user = context.Users.Include(x => x.Company).First(x => x.Id == userId);
            return await context.Trucks.Include(x => x.Class).Where(x => x.CompanyId == user.Company.Id).ToListAsync();
        }

        public async Task<List<Offer>> GetMyOffers(string userId)
        {
            var user = context.Users.Include(x => x.Company).First(x => x.Id == userId);
            return await context.Offers.Include(x => x.Category).Where(x => x.CompanyId == user.Company.Id).ToListAsync();
        }

        public async Task<List<TruckOffer>> GetMyTruckOffers(string userId)
        {
            var user = context.Users.Include(x => x.Company).First(x => x.Id == userId);
            return await context.TruckOffers.Include(x => x.Truck).Where(x => x.CompanyId == user.Company.Id).ToListAsync();
        }

        public async Task<Truck> GetTruckById(int truckId)
        {
            return await context.Trucks.FirstAsync(x => x.Id == truckId);
        }

        public async Task<List<Truck>> GetMyTrucksForOffer(string userId)
        {
            var user = context.Users.Include(x => x.Company).First(x => x.Id == userId);
            return await context.Trucks.Include(x => x.Class).Where(x => x.CompanyId == user.Company.Id && !x.IsForSale).ToListAsync();
        }

        public async Task<RatingViewModel> GetRating(string userId)
        {
            var user = await context.Users.Include(x => x.Company).FirstAsync(x => x.Id == userId);
            var model = new RatingViewModel()
            {
                AverageRating = user.Company.Rating,
                Count1 = context.CompanyRatings.Where(x => x.CompanyId == user.Company.Id && x.Rating==1).Count(),
                Count2 = context.CompanyRatings.Where(x => x.CompanyId == user.Company.Id && x.Rating==2).Count(),
                Count3 = context.CompanyRatings.Where(x => x.CompanyId == user.Company.Id && x.Rating==3).Count(),
                Count4 = context.CompanyRatings.Where(x => x.CompanyId == user.Company.Id && x.Rating==4).Count(),
                Count5 = context.CompanyRatings.Where(x => x.CompanyId == user.Company.Id && x.Rating==5).Count()
            };
            return model;
        }
    }
}
