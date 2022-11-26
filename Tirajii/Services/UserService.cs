using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tirajii.Data;
using Tirajii.Data.Models;
using Tirajii.Services.Contracts;

namespace Tirajii.Services
{
    public class UserService:IUserService
    {
        private readonly TruckersDbContext context;

        public UserService(TruckersDbContext _context)
        {
            context = _context;
        }

        public decimal GetBalanceByUserId(string userId)
        {
            var wallet = this.context
                       .Wallets
                       .FirstOrDefault(w => w.UserId == userId);

            if (wallet == null)
            {
                return -1;
            }

            return wallet.Balance;
        }
        public async Task<bool> ConnectWallet(string userId)
        {
            var user = await context.Users.FirstAsync(u => u.Id == userId);

            if (user.HasWallet)
            {
                return false;
            }

            user.HasWallet = true;
            user.Wallet = new Wallet
            {
                Balance = 0
            };

           await context.SaveChangesAsync();

            return true;
        }

        public bool HasWallet(string userId)
        =>  this.context
            .Users
            .Where(u => u.HasWallet == true)
            .Any(u => u.Id == userId);
    }
}
