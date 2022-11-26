using Tirajii.Data.Models;

namespace Tirajii.Services.Contracts
{
    public interface IUserService
    {
        public decimal GetBalanceByUserId(string userId);

        public Task<bool> ConnectWallet(string userId);

        public bool HasWallet(string userId);
    }
}
