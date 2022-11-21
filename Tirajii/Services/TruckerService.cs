using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.CopyAnalysis;
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

        public async Task<List<TruckingCategory>> GetAllTruckingCategories()
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

        public async Task RateACompany(string userId, int Id, int rating)
        {
            var company = context.Companies.First(x => x.Id == Id);
            var user = await context.Users.Include(x => x.Trucker).FirstAsync(x => x.Id == userId);
            if (await context.CompanyRatings.ContainsAsync(new CompanyRatings { RaterId = user.Trucker.Id, CompanyId = company.Id}))
            {
                throw new InvalidOperationException("Cannot rate an already rated company!");
            }
            company.Rating = ((decimal)context.CompanyRatings.Where(x => x.CompanyId == company.Id).Sum(x => x.Rating) + rating) / (context.CompanyRatings.Where(x => x.CompanyId == company.Id).Count()+1);
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
            Trucker trucker = new Trucker()
            {
                Name = model.FullName,
                User = user,
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

        public AllCompaniesViewModel GetAllCompanies(string category = null, string searchTerm = null, CompanySorting sorting = CompanySorting.Rating, int currentPage = 1)
        {
            var companies = this.context.Companies.Include(x => x.Owner).Where(c => true);

            if (!string.IsNullOrWhiteSpace(category))
            {
                companies = companies.Where(c => c.Category.Name == category);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                companies = companies.Where(c =>
                c.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            var totalCompanies = companies.Count();

            companies = sorting switch
            {
                CompanySorting.Rating => companies.OrderByDescending(c => c.Rating),
                CompanySorting.Name => companies.OrderBy(c => c.Name),
                _ => companies.OrderByDescending(c => c.Id)
            };
            return new AllCompaniesViewModel
            {
                TotalCompanies = totalCompanies,
                CurrentPage = currentPage,
                Companies = companies
            };
        }

        public async Task<List<CompanyCategory>> GetAllCompanyCategories()
        {
            return await context.CompanyCategories.ToListAsync();
        }
    }
}
