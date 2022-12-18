using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Runtime.InteropServices;
using Tirajii.Data;
using Tirajii.Data.Models;
using Tirajii.Models;
using Tirajii.Models.Company;
using Tirajii.Models.Trucker;
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
                Rating = 5
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

            var offerCompanyOwner = new User
            {
                Id = "221ac3a1-5502-45b6-b328-8c47c142341d",
                UserName = "peshko",
                IsOfferCompanyOwner = true,
                Email = "peshko@gmail.com"
            };
            var dummy = new User
            {
                Id = "221ac3a1-5502-45b6-b328-8c47c142341o",
                UserName = "dummy",
                Email = "dummy@gmail.com"
            };
            var truckerCompanyOwner = new User
            {
                Id = "221ac3a1-5502-45b6-b328-8c47c142341r",
                UserName = "toshko",
                IsTruckerCompanyOwner = true,
                Email = "toshko@gmail.com"
            };
            var truckerUser = new User
            {
                Id = "221ac3a1-5502-45b6-b328-8c47c142341e",
                UserName = "mishko",
                IsTrucker = true,
                Email = "mishko@gmail.com"
            };

            var claimedOffer = new Offer()
            {
                Name = "ClaimedOffer",
                DueDate = DateTime.UtcNow,
                Description = "deedfwlafa",
                IsTaken = true,
                IsApproved = true,
                CategoryId = 1,
                CompanyId = 1,
                TruckerId = 1,
                Payment = 1000
            };
            var unclaimedOffer = new Offer()
            {
                Name = "UnclaimedOffer",
                DueDate = DateTime.MaxValue,
                Description = "deedfwlafa",
                IsTaken = false,
                IsApproved = true,
                CategoryId = 1,
                CompanyId = 1,
                TruckerId = null,
                Payment = 100
            };
            var sortingOffer = new Offer()
            {
                Name = "SearchOffer",
                DueDate = DateTime.MinValue,
                Description = "deedfwlafa",
                IsTaken = false,
                IsApproved = true,
                CategoryId = 2,
                CompanyId = 1,
                TruckerId = null,
                Payment = 10
            };
            var unapprovedOffer = new Offer()
            {
                Name = "UnapprovedOffer",
                DueDate = DateTime.UtcNow.AddDays(100),
                Description = "deedfwlafa",
                IsTaken = false,
                IsApproved = false,
                CategoryId = 1,
                CompanyId = 1,
                TruckerId = null,
                Payment = 10
            };

            Truck truck1 = new Truck()
            {
                Id = 1,
                Name = "num1",
                CompanyId = 2,
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                ClassId = 2,
                Colour = "red",
                IsForSale = true,
                RegistrationNumber = "QQ 1234 OO",
                HasBluetooth = true,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasParkTronic = true,
                HasSpeakers = false
            };
            Truck truck2 = new Truck()
            {
                Id = 2,
                Name = "num2",
                CompanyId = 2,
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                ClassId = 1,
                Colour = "red",
                IsForSale = true,
                RegistrationNumber = "QQ 1234 TT",
                HasBluetooth = true,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasParkTronic = true,
                HasSpeakers = false
            };
            Truck truck3 = new Truck()
            {
                Id = 3,
                Name = "num3",
                CompanyId = 2,
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                ClassId = 3,
                Colour = "red",
                IsForSale = true,
                RegistrationNumber = "QQ 1234 QQ",
                HasBluetooth = true,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasParkTronic = true,
                HasSpeakers = false
            };
            Truck truck4 = new Truck()
            {
                Id = 4,
                Name = "num4",
                CompanyId = 2,
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                ClassId = 4,
                Colour = "red",
                IsForSale = true,
                RegistrationNumber = "QQ 1234 EE",
                HasBluetooth = true,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasParkTronic = true,
                HasSpeakers = false
            };

            var boughtTruckOffer = new TruckOffer()
            {
                Name = "BoughtOffer",
                IsApproved = true,
                IsBought = true,
                Cost = 1000,
                CompanyId = 2,
                Description = "fhyiawgfiuwh",
                TruckId = 1
            };
            var forSale = new TruckOffer()
            {
                Name = "OfferForSale",
                IsApproved = true,
                IsBought = false,
                Cost = 1000,
                CompanyId = 2,
                Description = "fhyiawgfiuwh",
                TruckId = 2
            };
            var sortingTruckOffer = new TruckOffer()
            {
                Name = "AAAAA",
                IsApproved = true,
                IsBought = false,
                Cost = 10,
                CompanyId = 2,
                Description = "fhyiawgfiuwh",
                TruckId = 3
            };
            var unapprovedTruckOffer = new TruckOffer()
            {
                Name = "AAAAA",
                IsApproved = false,
                IsBought = false,
                Cost = 10,
                CompanyId = 2,
                Description = "fhyiawgfiuwh",
                TruckId = 4
            };

            context.Users.Add(truckerCompanyOwner);
            context.Users.Add(offerCompanyOwner);
            context.Users.Add(truckerUser);
            context.Users.Add(dummy);

            context.Companies.Add(offerCompany);
            context.Companies.Add(truckerCompany);

            context.Truckers.Add(trucker);

            context.Offers.AddRange(new[] {claimedOffer, unclaimedOffer, sortingOffer, unapprovedOffer } );

            context.Trucks.AddRange(new[] { truck1, truck2, truck3, truck4 });

            context.TruckOffers.AddRange(new[] {boughtTruckOffer, forSale, sortingTruckOffer, unapprovedTruckOffer });

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

        [Test]
        public void GetAllTruckingCategoriesGetsAllCategories()
        {
            Assert.That(truckerService.GetAllTruckingCategories().Result.Count == 3);
        }
        
        [Test]
        public void GetAllTruckClassesGetsAllClasses()
        {
            Assert.That(truckerService.GetAllClasses().Result.Count == 5);
        }
        
        [Test]
        public void GetAllOffersGetsOffersByGivenCriteria()
        {
            var offers = truckerService.GetAllOffers("Food");
            Assert.That(offers.TotalOffers == 1 && offers.Offers.Last().CategoryId == 2);
            
            offers = truckerService.GetAllOffers(null, null, OfferSorting.Payment);
            Assert.That(offers.TotalOffers == 2 && offers.Offers.First().Payment == 100);
            
            offers = truckerService.GetAllOffers(null, "Unclaimed", OfferSorting.DueDate);
            Assert.That(offers.TotalOffers == 1 && offers.Offers.Last().DueDate == DateTime.MaxValue && offers.Offers.Last().Name == "UnclaimedOffer");
            
            offers = truckerService.GetAllOffers(null, null, OfferSorting.DueDate);
            Assert.That(offers.TotalOffers == 2 && offers.Offers.Last().DueDate == DateTime.MinValue);
        }
        
        [Test]
        public void GetAllTrcukOffersGetsTruckOffersByGivenCriteria()
        {
            var offers = truckerService.GetAllTruckOffers("BE");
            Assert.That(offers.TotalOffers == 1 && offers.Offers.Last().TruckId == 2);
            
            offers = truckerService.GetAllTruckOffers(null, null, TruckOfferSorting.Cost);
            Assert.That(offers.TotalOffers == 2 && offers.Offers.First().Cost == 10);
            
            offers = truckerService.GetAllTruckOffers(null, "ForSale", TruckOfferSorting.Name);
            Assert.That(offers.TotalOffers == 1 && offers.Offers.Last().Name == "OfferForSale");
            
            offers = truckerService.GetAllTruckOffers(null, null, TruckOfferSorting.Company);
            Assert.That(offers.TotalOffers == 2);
        }
        
        [Test]
        public void GetAllAllCompaniesGetsCompaniesByGivenCriteria()
        {
            var companies = truckerService.GetAllCompanies("OfferProvider");
            Assert.That(companies.TotalCompanies == 1 && companies.Companies.Last().Id == 1);
            
            companies = truckerService.GetAllCompanies(null, null, CompanySorting.Name);
            Assert.That(companies.TotalCompanies == 2 && companies.Companies.First().Name == "Kamioni");
            
            companies = truckerService.GetAllCompanies(null, "Kami", CompanySorting.Name);
            Assert.That(companies.TotalCompanies == 1 && companies.Companies.Last().Name == "Kamioni");
            
            companies = truckerService.GetAllCompanies(null, null, CompanySorting.Rating);
            Assert.That(companies.TotalCompanies == 2 && companies.Companies.First().Rating == 5);
        }
        
        [Test]
        public async Task RateACompanyRatesSavesAndThrowsExceptionIfUserHasAlreadyRated()
        {
            await truckerService.RateACompany("221ac3a1-5502-45b6-b328-8c47c142341e",2, 5);
            Assert.That(context.CompanyRatings.Count() == 1 && context.CompanyRatings.First().CompanyId == 2);

            Assert.That(truckerService.RateACompany("221ac3a1-5502-45b6-b328-8c47c142341e", 2, 5).Exception is not null);

        }
        
        [Test]
        public async Task RegisterTruckerRegistesATrucker()
        {
            var model = new TruckerRegisterViewModel()
            {
                FullName = "DummyDumov",
                Email = "dummy@gmail.com",
                CategoryId = 1,
                PhoneNumber = "+4564784556",
                Description = "gnhraohweuiwaj",
                Picture = null
            };
            await truckerService.RegisterTrucker(model, "221ac3a1-5502-45b6-b328-8c47c142341o");

            Assert.That(context.Truckers.Count() == 2 && context.Truckers.Last().Email == "dummy@gmail.com");
        }

        [Test]
        public async Task GetUserWithTruckerReturnsWhatItShouldOrGivesNullOrThrowsExceptionAtAnInvalidId()
        {
            var user = await truckerService.GetUserWithTrucker("221ac3a1-5502-45b6-b328-8c47c142341e");
            Assert.That(user.Trucker, Is.Not.Null);

            user = await truckerService.GetUserWithTrucker("221ac3a1-5502-45b6-b328-8c47c142341r");
            Assert.That(user.Trucker, Is.Null);

            Assert.That(truckerService.GetUserWithTrucker("221ac3a1-5502-45b6-b328-8c47c142341p").Exception, Is.Not.Null);
        }

        [Test]
        public void GetAllCompanyCategoriesGetsAllCategories()
        {
            Assert.That(truckerService.GetAllCompanyCategories().Result.Count == 2);
        }

        [Test]
        public async Task ClaimAnOfferAssignsAnOfferToATruckerOrThrowsExceptionAtInvalidIds()
        {
            await truckerService.ClaimAnOffer("221ac3a1-5502-45b6-b328-8c47c142341e", 2);
            Assert.That(context.Offers.First().IsTaken && context.Offers.First().TruckerId == 1);
            Assert.That(truckerService.ClaimAnOffer("221ac3a1-5502-45b6-b328-8c47c142341p", 2).Exception is not null);
            Assert.That(truckerService.ClaimAnOffer("221ac3a1-5502-45b6-b328-8c47c142341e", -1).Exception is not null);
        }
        
        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}
