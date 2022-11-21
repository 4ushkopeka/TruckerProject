using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Globalization;
using System.Security;
using Tirajii.Data;
using Tirajii.Data.Models;
using Tirajii.Models;
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

        public List<TruckClass> GetAllClasses()
        {
            return context.TruckClasses.ToList();
        }

        public AllOffersViewModel GetAllOffers(string category = null,
            string searchTerm = null,
            CollectionSorting sorting = CollectionSorting.DueDate,
            int currentPage = 1)
        {
            var offers = this.context.Offers.Where(c => true);

            if (!string.IsNullOrWhiteSpace(category))
            {
                offers = offers.Where(c => c.Category.Name == category);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                offers = offers.Where(c =>
                c.Name.ToLower().Contains(searchTerm.ToLower()) ||
                c.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            var totalOffers = offers.Count();

            offers = sorting switch
            {
                CollectionSorting.DueDate => offers.OrderByDescending(c => c.DueDate),
                CollectionSorting.Name => offers.OrderBy(c => c.Name),
                _ => offers.OrderByDescending(c => c.Id)
            };
            return new AllOffersViewModel
            {
                TotalOffers = totalOffers,
                CurrentPage = currentPage,
                Offers = offers.Include(x => x.Company).Include(y => y.Company)
            };
        }

        public AllTruckOffersViewModel GetAllTruckOffers(string category = null,
            string searchTerm = null,
            TruckOfferSorting sorting = TruckOfferSorting.Cost,
            int currentPage = 1)
        {
            var offers = this.context.TruckOffers.Include(x => x.Truck).Include(x => x.Company).Where(c => true);

            if (!string.IsNullOrWhiteSpace(category))
            {
                offers = offers.Where(c => c.Truck.Class.Name == category);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                offers = offers.Where(c =>
                c.Name.ToLower().Contains(searchTerm.ToLower()) ||
                c.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            var totalOffers = offers.Count();

            offers = sorting switch
            {
                TruckOfferSorting.Cost => offers.OrderByDescending(c => c.Cost),
                TruckOfferSorting.Name => offers.OrderBy(c => c.Name),
                TruckOfferSorting.Company => offers.OrderBy(c => c.Company.Name),
                _ => offers.OrderByDescending(c => c.Id)
            };
            return new AllTruckOffersViewModel
            {
                TotalOffers = totalOffers,
                CurrentPage = currentPage,
                Offers = offers
            };
        }

        public async Task<List<Company>> GetAllCompanies()
        {
            return await context.Companies.Include(x => x.Category).Include(x => x.Owner).ToListAsync();
        }
        public async Task RateACompany(string userId, int Id, int rating)
        {
            var company = context.Companies.First(x => x.Id == Id);
            var user = await context.Users.Include(x => x.Trucker).FirstAsync(x => x.Id == userId);
            if (await context.CompanyRatings.ContainsAsync(new CompanyRatings { RaterId = user.Trucker.Id, CompanyId = company.Id}))
            {
                throw new InvalidOperationException("Cannot rate an already rated company!");
            }
            await context.CompanyRatings.AddAsync(new CompanyRatings
            {
                CompanyId = company.Id,
                RaterId = user.Trucker.Id,
                Rating = rating
            });
            await context.SaveChangesAsync();
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

        public async Task<User> GetUserWithTrucker(string userId)
        {
            var user = await context.Users.Include(x => x.Trucker).FirstAsync(x => x.Id == userId);
            return user;
        }
    }
}
