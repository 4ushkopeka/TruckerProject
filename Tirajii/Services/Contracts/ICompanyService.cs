using Tirajii.Data.Models;
using Tirajii.Models.Company;
using Tirajii.Models.Trucker;

namespace Tirajii.Services.Contracts
{
    public interface ICompanyService
    {
        Task RegisterCompany(CompanyRegisterViewModel model, string userId);
        Task<List<CompanyCategory>> GetAllCategories();
        Task AddOffer(OfferAddViewModel model, string UserId);
        Task<List<TruckingCategory>> GetAllOfferCategories();
    }
}
