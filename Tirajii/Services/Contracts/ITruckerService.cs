using Tirajii.Data.Models;
using Tirajii.Models;
using Tirajii.Models.Trucker;

namespace Tirajii.Services.Contracts
{
    public interface ITruckerService
    {
        Task RegisterTrucker(TruckerRegisterViewModel model, string userId);

        Task EditTrucker(TruckerRegisterViewModel model, string userId);

        Task<List<TruckingCategory>> GetAllTruckingCategories();

        Task<List<CompanyCategory>> GetAllCompanyCategories();

        List<TruckClass> GetAllClasses();

        Task RateACompany(string userId, int Id, int rating);

        AllTruckOffersViewModel GetAllTruckOffers(string category = null,
            string searchTerm = null,
            TruckOfferSorting sorting = TruckOfferSorting.Cost,
            int currentPage = 1);

        AllOffersViewModel GetAllOffers(string category = null,
            string searchTerm = null,
            CollectionSorting sorting = CollectionSorting.DueDate,
            int currentPage = 1);

        AllCompaniesViewModel GetAllCompanies(string category = null,
            string searchTerm = null,
            CompanySorting sorting = CompanySorting.Rating,
            int currentPage = 1);

        Task<User> GetUserWithTrucker(string userId);
        
        Task<List<Offer>> GetMyOffers(string userId);

        Task<List<Offer>> GetMyCompletedOffers(string userId);
        
        Task ClaimAnOffer(string userId, int offerId);

        Task<ExperienceModel> OfferSucceed(string userId, int offerId);

        Task<ExperienceModel> FailOffer(string userId, int offerId);
    }
}
