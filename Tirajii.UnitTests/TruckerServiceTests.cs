using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Runtime.InteropServices;
using Tirajii.Data;
using Tirajii.Data.Models;
using Tirajii.Models.Company;
using Tirajii.Services;
using Tirajii.Services.Contracts;

namespace Tirajii.UnitTests
{
    [TestFixture]
    public class TruckerServiceTests
    {
        private TruckersDbContext context;
        private ITruckerService truckerService;

        [SetUp]
        public async Task Setup()
        {

            var contextOptions = new DbContextOptionsBuilder<TruckersDbContext>()
                .UseInMemoryDatabase("TruckersDB")
                .Options;

            context = new TruckersDbContext(contextOptions);
            truckerService = new TruckerService(context);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var offerCompany = new Company()
            {
                Id = 1,
                Name = "Offerti",
                CategoryId = 1,
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                OwnerId = "221ac3a1-5502-45b6-b328-8c47c142341d",
                Rating = 0
            };
            var truckerCompany = new Company()
            {
                Id = 2,
                Name = "Kamioni",
                CategoryId = 2,
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                OwnerId = "221ac3a1-5502-45b6-b328-8c47c142341r",
                Rating = 0
            };
            var trucker = new Trucker()
            {
                Id = 1,
                Name = "Tirajiq",
                Description = "efbwhfiwfirgiowehiogf",
                CategoryId = 1,
                Email = "peshko@gmail.com",
                Level = 0,
                Experience = 0,
                PhoneNumber = "+4578941548",
                ProfilePicture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                UserId = "221ac3a1-5502-45b6-b328-8c47c142341e",
                TruckId = null
            };

            var user = new User
            {
                Id = "221ac3a1-5502-45b6-b328-8c47c142341d",
                UserName = "peshko",
                IsOfferCompanyOwner = true,
                Email = "peshko@gmail.com"
            };
            var user3 = new User
            {
                Id = "221ac3a1-5502-45b6-b328-8c47c142341o",
                UserName = "dummy",
                IsOfferCompanyOwner = true,
                Email = "dummy@gmail.com"
            };
            var user1 = new User
            {
                Id = "221ac3a1-5502-45b6-b328-8c47c142341r",
                UserName = "toshko",
                IsTruckerCompanyOwner = true,
                Email = "toshko@gmail.com"
            };
            var user2 = new User
            {
                Id = "221ac3a1-5502-45b6-b328-8c47c142341e",
                UserName = "mishko",
                IsTrucker = true,
                Email = "mishko@gmail.com"
            };

            context.Users.Add(user1);
            context.Users.Add(user);
            context.Users.Add(user2);
            context.Users.Add(user3);

            context.Companies.Add(offerCompany);
            context.Companies.Add(truckerCompany);

            context.Truckers.Add(trucker);

            context.TruckClasses.AddRange(new[]
            {
                new TruckClass { Name = "BE" },
                new TruckClass { Name = "C" },
                new TruckClass { Name = "CE" },
                new TruckClass { Name = "C1" },
                new TruckClass { Name = "C1E" }
            });

            context.CompanyCategories.AddRange(new[]
            {
                new CompanyCategory { Name = "OfferProvider"},
                new CompanyCategory { Name = "TruckProvider"}
            });

            context.TruckingCategories.AddRange(new[]
            {
                new TruckingCategory { Name = "Livestock"},
                new TruckingCategory { Name = "Food"},
                new TruckingCategory { Name = "Items"}
            });

            await context.SaveChangesAsync();
        }



        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}
