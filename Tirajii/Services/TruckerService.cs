using Ganss.Xss;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.CopyAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Globalization;
using System.Security;
using System.Xml.XPath;
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
        private readonly HtmlSanitizer sanitizer = new();
        public TruckerService(TruckersDbContext context)
        {
            this.context = context;
        }

        public async Task<List<TruckingCategory>> GetAllTruckingCategories()
        {
            return await context.TruckingCategories.ToListAsync();
        }

        public async Task<List<TruckClass>> GetAllClasses()
        {
            return await context.TruckClasses.ToListAsync();
        }

        public AllOffersViewModel GetAllOffers(string category = null,
            string searchTerm = null,
            OfferSorting sorting = OfferSorting.DueDate,
            int currentPage = 1)
        {
            var offers = context.Offers.Where(c => !c.IsTaken && c.IsApproved);

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
                OfferSorting.DueDate => offers.OrderByDescending(c => c.DueDate),
                OfferSorting.Name => offers.OrderBy(c => c.Name),
                OfferSorting.Payment => offers.OrderByDescending(c => c.Payment),
                _ => offers.OrderByDescending(c => c.Id)
            };
            
            return new AllOffersViewModel
            {
                TotalOffers = totalOffers,
                CurrentPage = currentPage,
                Offers = offers.Include(x => x.Company)
            };
        }

        public AllTruckOffersViewModel GetAllTruckOffers(string category = null,
            string searchTerm = null,
            TruckOfferSorting sorting = TruckOfferSorting.Cost,
            int currentPage = 1)
        {
            var offers = this.context.TruckOffers.Include(x => x.Truck).Include(x => x.Company).Where(c => c.IsApproved && !c.IsBought);

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
                TruckOfferSorting.Cost => offers.OrderBy(c => c.Cost),
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

            model = SanitizeTrucker(model);
            Trucker trucker = new Trucker()
            {
                Name = model.FullName,
                User = user,
                CategoryId = model.CategoryId,
                Email = model.Email,
                ProfilePicture = model.Picture,
                PhoneNumber = model.PhoneNumber,
                Experience = 0,
                Level = 0,
                Description = model.Description
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

        public AllCompaniesViewModel GetAllCompanies(string category = null, 
            string searchTerm = null, 
            CompanySorting sorting = CompanySorting.Rating, 
            int currentPage = 1)
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

        public async Task ClaimAnOffer(string userId, int offerId)
        {
            var user = await context.Users.Include(x => x.Trucker).FirstAsync(x => x.Id == userId);
            var offer = await  context.Offers.FirstAsync(x => x.Id == offerId);
            
            user.Trucker.Offers.Add(offer);
            offer.IsTaken = true;
            offer.Trucker = user.Trucker;
            
            await context.SaveChangesAsync();
        }

        public async Task<List<Offer>> GetMyOffers(string userId)
        {
            var user = await context.Users.Include(x => x.Trucker).FirstAsync(x => x.Id == userId);
            
            return await context.Offers.Include(x => x.Company).Include(x => x.Category).Where(x => x.TruckerId == user.Trucker.Id && !x.IsCompleted).ToListAsync();
        }
        public async Task<List<Offer>> GetMyCompletedOffers(string userId)
        {
            var user = await context.Users.Include(x => x.Trucker).FirstAsync(x => x.Id == userId);
            
            return await context.Offers.Include(x => x.Company).Include(x => x.Category).Where(x => x.TruckerId == user.Trucker.Id && x.IsCompleted).ToListAsync();
        }

        public async Task<ExperienceModel> OfferSucceed(string userId, int offerId)
        {
            var user = await context.Users.Include(x => x.Trucker).Include(x => x.Wallet).FirstAsync(x => x.Id == userId);
            var offer = await context.Offers.FirstAsync(x => x.Id == offerId);
            var company = await context.Companies.Include(x => x.Owner).ThenInclude(x => x.Wallet).FirstAsync(x => x.Id == offer.CompanyId);
            
            user.Trucker.Experience += offer.ExpAmount;
            offer.IsCompleted = true;
            user.Wallet.Balance += offer.Payment;
            company.Owner.Wallet.Balance += 1.30M * offer.Payment;

            if (user.Trucker.Experience >= 10)
            {
                user.Trucker.Level++; user.Trucker.Experience -= 10;
                await context.SaveChangesAsync();
                return new ExperienceModel()
                {
                    WasLeveledUp = true,
                    Level = user.Trucker.Level
                };
            }

            await context.SaveChangesAsync();

            return new ExperienceModel()
            {
                GainedXp = true,
                Xp = user.Trucker.Experience
            };
        }

        public async Task<ExperienceModel> FailOffer(string userId, int offerId)
        {
            var user = await context.Users.Include(x => x.Trucker).Include(x => x.Wallet).FirstAsync(x => x.Id == userId);
            var offer = await context.Offers.FirstAsync(x => x.Id == offerId);

            user.Trucker.Experience -= offer.ExpAmount*2;
            offer.IsTaken = false;
            offer.TruckerId = null;
            user.Wallet.Balance-= offer.Payment/5;

            if (user.Wallet.Balance < 0) user.Wallet.Balance = 0;

            if (user.Trucker.Experience <= 0) 
            { 
                user.Trucker.Level--; 
                user.Trucker.Experience = 0; 
                await context.SaveChangesAsync(); 
                return new ExperienceModel()
                {
                    WasLeveledUp = true
                };
            }
            await context.SaveChangesAsync();
            return new ExperienceModel()
            {
                GainedXp = true,
                Xp = offer.ExpAmount * 2
            };
        }

        public async Task<bool> Purchase(int truckId, string userId)
        {
            var user = await GetUserWithTrucker(userId);
            var wallet = await context.Wallets.FirstAsync(u => u.UserId == userId);
            var offer = await context.TruckOffers.Include(x => x.Truck).FirstAsync(n => n.Truck.Id == truckId);
            var company = await context.Companies.FirstAsync(u => u.Id == offer.CompanyId);
            var ownerWallet = await context.Wallets.FirstAsync(w => w.UserId == company.OwnerId);

            if (offer.Cost > wallet.Balance)
            {
                return false;
            }

            offer.Truck.OwnerId = userId;
            offer.Truck.IsForSale = false;
            ownerWallet.Balance += offer.Cost*1.35M;
            offer.Truck.Owner = user;
            wallet.Balance -= offer.Cost;
            user.Trucker.TruckId = truckId;
            offer.IsBought = true;
            await context.SaveChangesAsync();
            return true;
        }
        public async Task EditTrucker(TruckerRegisterViewModel model, string userId)
        {
            var user = await context.Users.Include(x => x.Trucker).FirstAsync(x => x.Id == userId);
            var trucker = user.Trucker;

            model = SanitizeTrucker(model);
            trucker.PhoneNumber = model.PhoneNumber;
            trucker.Description = model.Description;
            trucker.Email = model.Email;
            trucker.ProfilePicture = model.Picture;
            trucker.Name = model.FullName;
            trucker.PhoneNumber = model.PhoneNumber;

            await context.SaveChangesAsync();
        }

        private TruckerRegisterViewModel SanitizeTrucker(TruckerRegisterViewModel model)
        {
            model.Picture = sanitizer.Sanitize(model.Picture is null ? "" : model.Picture);
            model.PhoneNumber = sanitizer.Sanitize(model.PhoneNumber);
            model.Description = sanitizer.Sanitize(model.Description);
            model.FullName = sanitizer.Sanitize(model.FullName);
            
            return model;
        }
    }
}
