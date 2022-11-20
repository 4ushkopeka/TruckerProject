using Tirajii.Data.Models;
using Tirajii.Models;
using Tirajii.Models.Trucker;

namespace Tirajii.Services.Contracts
{
    public interface ITruckerService
    {
        Task RegisterTrucker(TruckerRegisterViewModel model, string userId);

        Task<List<TruckingCategory>> GetAllCategories();

        List<TruckClass> GetAllClasses();

        AllTruckOffersViewModel GetAllTruckOffers(string category = null,
            string searchTerm = null,
            TruckOfferSorting sorting = TruckOfferSorting.Cost,
            int currentPage = 1);

        AllOffersViewModel GetAllOffers(string category = null,
            string searchTerm = null,
            CollectionSorting sorting = CollectionSorting.DueDate,
            int currentPage = 1);
    }
}
