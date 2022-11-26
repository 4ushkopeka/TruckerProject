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

        public async Task AddOffer(OfferAddNEditViewModel model, string userId)
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
            var user = await context.Users.FirstAsync(x => x.Id == userId);
            
            Company company = new Company()
            {
                Name = model.Name,
                Owner = user,
                Picture = model.Picture,
                CategoryId = model.CategoryId
            };

            if (model.CategoryId == context.CompanyCategories.Max(x => x.Id)-1) user.IsOfferCompanyOwner = true;
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
        public async Task AddTruckOffer(TruckOfferAddNEditViewModel model, string userId)
        {
            var user = context.Users.Include(x => x.Company).First(x => x.Id == userId);
            var truck = await this.GetTruckById(model.TruckId);
            
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
            return await context.Trucks.Include(x => x.Company).FirstAsync(x => x.Id == truckId);
        }

        public async Task<List<Truck>> GetMyTrucksForOffer(string userId)
        {
            var user = context.Users.Include(x => x.Company).First(x => x.Id == userId);
            
            return await context.Trucks.Include(x => x.Class).Where(x => x.CompanyId == user.Company.Id && !x.IsForSale).ToListAsync();
        }

        public async Task<RatingViewModel> GetRating(string userId)
        {
            var user = await context.Users.Include(x => x.Company).FirstAsync(x => x.Id == userId);
            var grouping = context.CompanyRatings.GroupBy(x => new {x.Rating, x.CompanyId}).Select(x => new { Rating = x.Key.Rating, CompanyId = x.Key.CompanyId, Count = x.Count() }).ToDictionary(k => new { k.Rating, k.CompanyId }, i => i.Count);
            
            var model = new RatingViewModel()
            {
                AverageRating = user.Company.Rating,
                Count1 = grouping.ContainsKey(new { Rating = 1, CompanyId = user.Company.Id }) ? grouping[new { Rating = 1, CompanyId = user.Company.Id }] : 0,
                Count2 = grouping.ContainsKey(new { Rating = 2, CompanyId = user.Company.Id }) ? grouping[new { Rating = 2, CompanyId = user.Company.Id }] : 0,
                Count3 = grouping.ContainsKey(new { Rating = 3, CompanyId = user.Company.Id }) ? grouping[new { Rating = 3, CompanyId = user.Company.Id }] : 0,
                Count4 = grouping.ContainsKey(new { Rating = 4, CompanyId = user.Company.Id }) ? grouping[new { Rating = 4, CompanyId = user.Company.Id }] : 0,
                Count5 = grouping.ContainsKey(new { Rating = 5, CompanyId = user.Company.Id }) ? grouping[new { Rating = 5, CompanyId = user.Company.Id }] : 0
            };
            
            return model;
        }

        public async Task EditCompany(CompanyRegisterViewModel model, string userId)
        {
            var user = await context.Users.Include(x => x.Company).FirstAsync(x => x.Id == userId);
            var company = user.Company;

            company.Picture = model.Picture;
            company.Name = model.Name;
            
            await context.SaveChangesAsync();
        }

        public async Task<User> GetUserWithCompany(string userId)
        {
            return await context.Users.Include(x => x.Company).FirstAsync(x => x.Id == userId);
        }

        public async Task EditOffer(OfferAddNEditViewModel model, int offerId)
        {
            var offer = await context.Offers.FirstAsync(x => x.Id == offerId);
            
            bool date = DateTime.TryParseExact(model.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime BDayval);
            if (!date)
            {
                throw new ArgumentException("Invalid date format!");
            }
            
            offer.Description = model.Description;
            offer.DueDate = BDayval;
            offer.CategoryId = model.CategoryId;
            offer.Name = model.Name;
            offer.Payment = model.Payment;
            
            await context.SaveChangesAsync();
        }

        public async Task EditTruckOffer(TruckOfferAddNEditViewModel model, int truckOfferId)
        {
            var offer = await context.TruckOffers.FirstAsync(x => x.Id == truckOfferId);
            
            offer.Description = model.Description;
            offer.Name = model.Name;
            offer.Cost = model.Cost;
            
            await context.SaveChangesAsync();
        }

        public async Task<Offer> GetOfferById(int offerId)
        {
            return await context.Offers.FirstAsync(x => x.Id == offerId);
        }

        public async Task<TruckOffer> GetTruckOfferById(int truckOfferId)
        {
            return await context.TruckOffers.FirstAsync(x => x.Id == truckOfferId);
        }

        public async Task DeleteTruckOffer(int truckOfferId)
        {
            var offer = await context.TruckOffers.Include(x => x.Truck).FirstAsync(x => x.Id == truckOfferId);
            offer.Truck.IsForSale = false;
            
            context.Remove(offer);
            await context.SaveChangesAsync();
        }

        public async Task DeleteOffer(int offerId)
        {
            var offer = await context.Offers.FirstAsync(x => x.Id == offerId);
           
            context.Remove(offer);
            await context.SaveChangesAsync();
        }
    }
}
