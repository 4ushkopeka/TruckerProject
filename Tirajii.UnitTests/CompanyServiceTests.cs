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
    public class CompanyServiceTests
    {
        private TruckersDbContext context;
        private ICompanyService companyService;

        [SetUp]
        public async Task Setup()
        {
            
            var contextOptions = new DbContextOptionsBuilder<TruckersDbContext>()
                .UseInMemoryDatabase("TruckersDB")
                .Options;
                
            context = new TruckersDbContext(contextOptions);
            companyService = new CompanyService(context);

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
            var truckerUser= new User
            {
                Id = "221ac3a1-5502-45b6-b328-8c47c142341e",
                UserName = "mishko",
                IsTrucker = true,
                Email = "mishko@gmail.com"
            };

            context.Users.Add(offerCompanyOwner);
            context.Users.Add(truckerCompanyOwner);
            context.Users.Add(truckerUser);
            context.Users.Add(dummy);

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

        [Test]
        public async Task AddOfferAddsAnOffer()
        {
            OfferAddNEditViewModel offerAddNEditViewModel = new OfferAddNEditViewModel()
            {
                Name = "n",
                Description = "nccccccccccccccc",
                CategoryId = 1,
                CompanyId = null,
                DueDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Payment = 1
            };
            
            await companyService.AddOffer(offerAddNEditViewModel, "221ac3a1-5502-45b6-b328-8c47c142341d");

            Assert.That(context.Offers.ToArray().Count(), Is.EqualTo(1));
        }

        [Test]
        public void AddOfferThrowsExceptionAtAnInvalidDateFormat()
        {
            OfferAddNEditViewModel offerAddNEditViewModel = new OfferAddNEditViewModel()
            {
                Name = "n",
                Description = "nccccccccccccccc",
                CategoryId = 1,
                CompanyId = null,
                DueDate = DateTime.UtcNow.ToString(),
                Payment = 1
            };

            Assert.That(companyService.AddOffer(offerAddNEditViewModel, "221ac3a1-5502-45b6-b328-8c47c142341d").Exception, Is.Not.Null);
        }

        [Test]
        public async Task GetAllCompanyCategoriesGetsCategories()
        {
            var categories = await companyService.GetAllCompanyCategories();

            Assert.That(categories.Count==2);
        }

        [Test]
        public async Task GetAllOfferCategoriesGetsOfferCategories()
        {
            var categories = await companyService.GetAllOfferCategories();

            Assert.That(categories.Count == 3);
        }

        [Test]
        public async Task GetAllTruckClassesGetsClasses()
        {
            var categories = await companyService.GetAllClasses();

            Assert.That(categories.Count == 5);
        }
        
        [Test]
        public async Task RegisterCompanyRegistersAnOfferCompany()
        {
            var company = new CompanyRegisterViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                Name = "oshitki",
                CategoryId = 1
            };
            await companyService.RegisterCompany(company, "221ac3a1-5502-45b6-b328-8c47c142341o");

            Assert.That(context.Companies.Count() == 3 && context.Users.First(x => x.Id == "221ac3a1-5502-45b6-b328-8c47c142341o").IsOfferCompanyOwner);
        }
        
        [Test]
        public async Task RegisterCompanyRegistersATruckCompany()
        {
            var company = new CompanyRegisterViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                Name = "oshitki",
                CategoryId = 2
            };
            await companyService.RegisterCompany(company, "221ac3a1-5502-45b6-b328-8c47c142341o");

            Assert.That(context.Companies.Count() == 3 && context.Users.First(x => x.Id == "221ac3a1-5502-45b6-b328-8c47c142341o").IsTruckerCompanyOwner);
        }
        
        [Test]
        public async Task RegisterTruckRegistersATruck()
        {
            var truck = new TruckViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                HasParkTronic = false,
                HasBluetooth = false,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasSpeakers = false,
                RegistrationNumber = "QQ 1234 EE",
                ClassId = 2,
                Colour = "red",
                CompanyId = 2,
                Name = "fkiojrwh"
            };
            await companyService.RegisterTruck(truck, "221ac3a1-5502-45b6-b328-8c47c142341r");

            Assert.That(context.Trucks.Count() == 1 && context.Trucks.First().Name == "fkiojrwh" && context.Trucks.First().CompanyId == 2);
        }

        [Test]
        public async Task AddTruckOfferAddsATruckOffer()
        {
            var truck = new TruckViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                HasParkTronic = false,
                HasBluetooth = false,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasSpeakers = false,
                RegistrationNumber = "QQ 1234 EE",
                ClassId = 2,
                Colour = "red",
                CompanyId = 2,
                Name = "fkiojrwh"
            };
            await companyService.RegisterTruck(truck, "221ac3a1-5502-45b6-b328-8c47c142341r");

            var offer = new TruckOfferAddNEditViewModel()
            {
                Description = "ghaklehfuiozahjfewn",
                Cost = 10000,
                Name = "TruckOffertichka",
                CompanyId = 2,
                TruckId = 1,
            };
            await companyService.AddTruckOffer(offer, "221ac3a1-5502-45b6-b328-8c47c142341r");

            Assert.That(context.TruckOffers.Count()==1 && context.TruckOffers.First().CompanyId == 2);

        }
        
        [Test]
        public async Task AddTruckOfferThrowsExceptionAtInvalidTruckId()
        {
            var truck = new TruckViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                HasParkTronic = false,
                HasBluetooth = false,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasSpeakers = false,
                RegistrationNumber = "QQ 1234 EE",
                ClassId = 2,
                Colour = "red",
                CompanyId = 2,
                Name = "fkiojrwh"
            };
            await companyService.RegisterTruck(truck, "221ac3a1-5502-45b6-b328-8c47c142341r");

            var offer = new TruckOfferAddNEditViewModel()
            {
                Description = "ghaklehfuiozahjfewn",
                Cost = 10000,
                Name = "TruckOffertichka",
                CompanyId = 2,
                TruckId = 212,
            };
            Assert.That(companyService.AddTruckOffer(offer, "221ac3a1-5502-45b6-b328-8c47c142341r").Exception, Is.Not.Null);
        }

        [Test]
        public async Task GetMyOffersGetsOnlyMyOffers()
        {
            OfferAddNEditViewModel offerAddNEditViewModel = new OfferAddNEditViewModel()
            {
                Name = "nnn",
                Description = "nccccccccccccccc",
                CategoryId = 1,
                CompanyId = null,
                DueDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Payment = 1
            };

            await companyService.AddOffer(offerAddNEditViewModel, "221ac3a1-5502-45b6-b328-8c47c142341d");
            
            OfferAddNEditViewModel offerAddNEditViewModel1 = new OfferAddNEditViewModel()
            {
                Name = "n",
                Description = "nccccccccccccccc",
                CategoryId = 2,
                CompanyId = null,
                DueDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Payment = 1000
            };

            await companyService.AddOffer(offerAddNEditViewModel1, "221ac3a1-5502-45b6-b328-8c47c142341d");

            var mine = await companyService.GetMyOffers("221ac3a1-5502-45b6-b328-8c47c142341d");

            Assert.That(mine.Count == 2 && mine.First().Name == "nnn" && mine.Last().Name == "n" && mine.Last().Category is not null);
        }
        
        [Test]
        public async Task GetMyOffersReturnsEmptyListForWrongId()
        {
            OfferAddNEditViewModel offerAddNEditViewModel = new OfferAddNEditViewModel()
            {
                Name = "nnn",
                Description = "nccccccccccccccc",
                CategoryId = 1,
                CompanyId = null,
                DueDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Payment = 1
            };

            await companyService.AddOffer(offerAddNEditViewModel, "221ac3a1-5502-45b6-b328-8c47c142341d");
            
            OfferAddNEditViewModel offerAddNEditViewModel1 = new OfferAddNEditViewModel()
            {
                Name = "n",
                Description = "nccccccccccccccc",
                CategoryId = 2,
                CompanyId = null,
                DueDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Payment = 1000
            };

            await companyService.AddOffer(offerAddNEditViewModel1, "221ac3a1-5502-45b6-b328-8c47c142341d");

            var mine = await companyService.GetMyOffers("221ac3a1-5502-45b6-b328-8c47c142341r");

            Assert.That(mine.Count == 0);
        }
        
        [Test]
        public async Task GetMyTrucksGetsOnlyMyTrucks()
        {
            var truck = new TruckViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                HasParkTronic = false,
                HasBluetooth = false,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasSpeakers = false,
                RegistrationNumber = "QQ 1234 EE",
                ClassId = 2,
                Colour = "red",
                CompanyId = 2,
                Name = "fkiojrwh"
            };
            await companyService.RegisterTruck(truck, "221ac3a1-5502-45b6-b328-8c47c142341r");

            var truck1 = new TruckViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                HasParkTronic = false,
                HasBluetooth = false,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasSpeakers = false,
                RegistrationNumber = "QQ 1234 EE",
                ClassId = 4,
                Colour = "red",
                CompanyId = 2,
                Name = "fkiojrwhhh"
            };
            await companyService.RegisterTruck(truck1, "221ac3a1-5502-45b6-b328-8c47c142341r");

            var mine = await companyService.GetMyTrucks("221ac3a1-5502-45b6-b328-8c47c142341r");

            Assert.That(mine.Count == 2 && mine.Last().Name == "fkiojrwhhh" && mine.First().Class is not null);
        }
        
        [Test]
        public async Task GetMyTrucksReturnsEmptyListForWrongId()
        {
            var truck = new TruckViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                HasParkTronic = false,
                HasBluetooth = false,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasSpeakers = false,
                RegistrationNumber = "QQ 1234 EE",
                ClassId = 2,
                Colour = "red",
                CompanyId = 2,
                Name = "fkiojrwh"
            };
            await companyService.RegisterTruck(truck, "221ac3a1-5502-45b6-b328-8c47c142341r");

            var truck1 = new TruckViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                HasParkTronic = false,
                HasBluetooth = false,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasSpeakers = false,
                RegistrationNumber = "QQ 1234 EE",
                ClassId = 4,
                Colour = "red",
                CompanyId = 2,
                Name = "fkiojrwhhh"
            };
            await companyService.RegisterTruck(truck1, "221ac3a1-5502-45b6-b328-8c47c142341r");

            var mine = await companyService.GetMyTrucks("221ac3a1-5502-45b6-b328-8c47c142341d");

            Assert.That(mine.Count == 0);
        }

        [Test]
        public async Task GetMyTruckOffersReturnsEmptyListOnWrongId()
        {
            var truck = new TruckViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                HasParkTronic = false,
                HasBluetooth = false,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasSpeakers = false,
                RegistrationNumber = "QQ 1234 EE",
                ClassId = 2,
                Colour = "red",
                CompanyId = 2,
                Name = "fkiojrwh"
            };
            await companyService.RegisterTruck(truck, "221ac3a1-5502-45b6-b328-8c47c142341r");

            var truck1 = new TruckViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                HasParkTronic = false,
                HasBluetooth = false,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasSpeakers = false,
                RegistrationNumber = "QQ 1234 EE",
                ClassId = 4,
                Colour = "red",
                CompanyId = 2,
                Name = "fkiojrwhhh"
            };
            await companyService.RegisterTruck(truck1, "221ac3a1-5502-45b6-b328-8c47c142341r");

            var offer = new TruckOfferAddNEditViewModel()
            {
                Description = "ghaklehfuiozahjfewn",
                Cost = 10000,
                Name = "TruckOffertichka",
                CompanyId = 2,
                TruckId = 1,
            };
            await companyService.AddTruckOffer(offer, "221ac3a1-5502-45b6-b328-8c47c142341r");

            var offer1 = new TruckOfferAddNEditViewModel()
            {
                Description = "ghaklehfuiozahjfewn",
                Cost = 20000,
                Name = "TruckOffertichkaaa",
                CompanyId = 2,
                TruckId = 2,
            };
            await companyService.AddTruckOffer(offer1, "221ac3a1-5502-45b6-b328-8c47c142341r");

            var mine = await companyService.GetMyTruckOffers("221ac3a1-5502-45b6-b328-8c47c142341d");

            Assert.That(mine.Count == 0);
        }
        
        [Test]
        public async Task GetMyTruckOffersGetsOnlyMyTruckOffers()
        {
            var truck = new TruckViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                HasParkTronic = false,
                HasBluetooth = false,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasSpeakers = false,
                RegistrationNumber = "QQ 1234 EE",
                ClassId = 2,
                Colour = "red",
                CompanyId = 2,
                Name = "fkiojrwh"
            };
            await companyService.RegisterTruck(truck, "221ac3a1-5502-45b6-b328-8c47c142341r");

            var truck1 = new TruckViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                HasParkTronic = false,
                HasBluetooth = false,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasSpeakers = false,
                RegistrationNumber = "QQ 1234 EE",
                ClassId = 4,
                Colour = "red",
                CompanyId = 2,
                Name = "fkiojrwhhh"
            };
            await companyService.RegisterTruck(truck1, "221ac3a1-5502-45b6-b328-8c47c142341r");

            var offer = new TruckOfferAddNEditViewModel()
            {
                Description = "ghaklehfuiozahjfewn",
                Cost = 10000,
                Name = "TruckOffertichka",
                CompanyId = 2,
                TruckId = 1,
            };
            await companyService.AddTruckOffer(offer, "221ac3a1-5502-45b6-b328-8c47c142341r");

            var offer1 = new TruckOfferAddNEditViewModel()
            {
                Description = "ghaklehfuiozahjfewn",
                Cost = 20000,
                Name = "TruckOffertichkaaa",
                CompanyId = 2,
                TruckId = 2,
            };
            await companyService.AddTruckOffer(offer1, "221ac3a1-5502-45b6-b328-8c47c142341r");

            var mine = await companyService.GetMyTruckOffers("221ac3a1-5502-45b6-b328-8c47c142341r");

            Assert.That(mine.Count == 2 && mine.Last().Name == "TruckOffertichkaaa" && mine.First().Cost == 10000 && mine.Last().Truck is not null);
        }

        [Test]
        public async Task GetTruckByIdGetsTruckById()
        {
            var truck = new TruckViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                HasParkTronic = false,
                HasBluetooth = false,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasSpeakers = false,
                RegistrationNumber = "QQ 1234 EE",
                ClassId = 2,
                Colour = "red",
                CompanyId = 2,
                Name = "fkiojrwh"
            };
            await companyService.RegisterTruck(truck, "221ac3a1-5502-45b6-b328-8c47c142341r");

            var truckche = await companyService.GetTruckById(1);
            Assert.That(truckche.ClassId == 2 && truckche.Colour == "red" && truckche.RegistrationNumber == "QQ 1234 EE");
        }
        
        [Test]
        public async Task GetTruckByIdThrowsExceptionAtWrongId()
        {
            var truck = new TruckViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                HasParkTronic = false,
                HasBluetooth = false,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasSpeakers = false,
                RegistrationNumber = "QQ 1234 EE",
                ClassId = 2,
                Colour = "red",
                CompanyId = 2,
                Name = "fkiojrwh"
            };
            await companyService.RegisterTruck(truck, "221ac3a1-5502-45b6-b328-8c47c142341r");

            Assert.That(companyService.GetTruckById(3).Exception, Is.Not.Null);
        }
        
        [Test]
        public async Task GetMyTrucksForOfferGetsOnlyTrucksForSale()
        {
            var truck = new TruckViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                HasParkTronic = false,
                HasBluetooth = false,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasSpeakers = false,
                RegistrationNumber = "QQ 1234 EE",
                ClassId = 2,
                Colour = "red",
                CompanyId = 2,
                Name = "fkiojrwh"
            };
            await companyService.RegisterTruck(truck, "221ac3a1-5502-45b6-b328-8c47c142341r");

            var truck1 = new TruckViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                HasParkTronic = false,
                HasBluetooth = false,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasSpeakers = false,
                RegistrationNumber = "QQ 1234 EE",
                ClassId = 4,
                Colour = "red",
                CompanyId = 2,
                Name = "fkiojrwhhh"
            };
            await companyService.RegisterTruck(truck1, "221ac3a1-5502-45b6-b328-8c47c142341r");
            var truckche = await companyService.GetTruckById(2);
            truckche.IsForSale = true;

            await context.SaveChangesAsync();

            var trucksForSale = await companyService.GetMyTrucksForOffer("221ac3a1-5502-45b6-b328-8c47c142341r");
            Assert.That(trucksForSale.Count == 1);
        }
        
        [Test]
        public async Task GetRatingReturnsValidRatingPerCompany()
        {
            var rating = new CompanyRatings()
            {
                RaterId = 1,
                CompanyId=1,
                Rating = 5
            };

            var rating1 = new CompanyRatings()
            {
                RaterId = 1,
                CompanyId = 2,
                Rating = 2
            };
            context.CompanyRatings.Add(rating);
            context.CompanyRatings.Add(rating1);
            await context.SaveChangesAsync();

            var ratings = await companyService.GetRating("221ac3a1-5502-45b6-b328-8c47c142341r");
            Assert.That(ratings.Count2 == 1);
        }

        [Test]
        public async Task EditCompanyEditsAndSavesProfile()
        {
            var company = context.Companies.First(x => x.OwnerId == "221ac3a1-5502-45b6-b328-8c47c142341r");
            Assert.That(company.Name == "Kamioni");
            var companyModel = new CompanyRegisterViewModel()
            {
                Picture = company.Picture,
                Name = "New Name",
                CategoryId = company.CategoryId,
            };

            await companyService.EditCompany(companyModel, "221ac3a1-5502-45b6-b328-8c47c142341r");

            Assert.That(company.Name == "New Name");
        }
        
        [Test]
        public async Task GetUserWithCompanyGetsUserWithCompanyAndThrowsAnExceptionAtAnInvalidId()
        {
            var user = await companyService.GetUserWithCompany("221ac3a1-5502-45b6-b328-8c47c142341r");

            Assert.That(user.Company is not null && user.Company.Name == "Kamioni");
            
            Assert.That(companyService.GetUserWithCompany("221ac3a1-5502-45b6-b328-8c47c142341y").Exception is not null);
        }

        [Test]
        public async Task EditOfferEditsAndSavesChanges()
        {
            OfferAddNEditViewModel offerAddNEditViewModel = new OfferAddNEditViewModel()
            {
                Name = "n",
                Description = "nccccccccccccccc",
                CategoryId = 1,
                CompanyId = null,
                DueDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Payment = 1
            };

            await companyService.AddOffer(offerAddNEditViewModel, "221ac3a1-5502-45b6-b328-8c47c142341d");
            Assert.That(context.Offers.First().Payment == 1);

            var editModel = new OfferAddNEditViewModel()
            {
                Name = "n",
                Description = "nccccccccccccccc",
                CategoryId = 1,
                CompanyId = null,
                DueDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Payment = 1000
            };

            await companyService.EditOffer(editModel, 1);

            Assert.That(context.Offers.First().Payment == 1000);
            
            var editModel2 = new OfferAddNEditViewModel()
            {
                Name = "n",
                Description = "nccccccccccccccc",
                CategoryId = 1,
                CompanyId = null,
                DueDate = DateTime.UtcNow.ToString("yyyy/MM/dd"),
                Payment = 1000
            };

            Assert.That(companyService.EditOffer(editModel2, 1).Exception is not null);
        }

        [Test]
        public async Task EditTruckOfferEditsAndSavesChanges()
        {
            var truck = new TruckViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                HasParkTronic = false,
                HasBluetooth = false,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasSpeakers = false,
                RegistrationNumber = "QQ 1234 EE",
                ClassId = 2,
                Colour = "red",
                CompanyId = 2,
                Name = "fkiojrwh"
            };
            await companyService.RegisterTruck(truck, "221ac3a1-5502-45b6-b328-8c47c142341r");

            var offer = new TruckOfferAddNEditViewModel()
            {
                Description = "ghaklehfuiozahjfewn",
                Cost = 10000,
                Name = "TruckOffertichka",
                CompanyId = 2,
                TruckId = 1,
            };
            await companyService.AddTruckOffer(offer, "221ac3a1-5502-45b6-b328-8c47c142341r");

            Assert.That(context.TruckOffers.Count() == 1 && context.TruckOffers.First().Cost == 10000);

            var editModel = new TruckOfferAddNEditViewModel()
            {
                Description = "ghaklehfuiozahjfewn",
                Cost = 1,
                Name = "TruckOffertichka",
                CompanyId = 2,
                TruckId = 1,
            };
            await companyService.EditTruckOffer(editModel, 1);

            Assert.That(context.TruckOffers.Count() == 1 && context.TruckOffers.First().Cost == 1);
        }

        [Test]
        public async Task GetOfferByIdReturningOfferAndExceptionAtAnInvalidId()
        {
            OfferAddNEditViewModel offerAddNEditViewModel = new OfferAddNEditViewModel()
            {
                Name = "n",
                Description = "nccccccccccccccc",
                CategoryId = 1,
                CompanyId = null,
                DueDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Payment = 1
            };

            await companyService.AddOffer(offerAddNEditViewModel, "221ac3a1-5502-45b6-b328-8c47c142341d");
            Assert.That(context.Offers.First().Payment == 1);

            var offer = await companyService.GetOfferById(1);
            Assert.That(offer.Payment == 1);

            Assert.That(companyService.GetOfferById(-1).Exception is not null);

        }
        
        [Test]
        public async Task GetTruckOfferByIdReturningTruckOfferAndExceptionAtAnInvalidId()
        {
            var truck = new TruckViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                HasParkTronic = false,
                HasBluetooth = false,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasSpeakers = false,
                RegistrationNumber = "QQ 1234 EE",
                ClassId = 2,
                Colour = "red",
                CompanyId = 2,
                Name = "fkiojrwh"
            };
            await companyService.RegisterTruck(truck, "221ac3a1-5502-45b6-b328-8c47c142341r");

            var offer = new TruckOfferAddNEditViewModel()
            {
                Description = "ghaklehfuiozahjfewn",
                Cost = 10000,
                Name = "TruckOffertichka",
                CompanyId = 2,
                TruckId = 1,
            };
            await companyService.AddTruckOffer(offer, "221ac3a1-5502-45b6-b328-8c47c142341r");

            Assert.That(context.TruckOffers.Count() == 1 && context.TruckOffers.First().CompanyId == 2);

            var truckOffer = await companyService.GetTruckOfferById(1);
            Assert.That(truckOffer.Cost == 10000 && truckOffer.Description == "ghaklehfuiozahjfewn");
            Assert.That(companyService.GetTruckOfferById(-1).Exception is not null);
        }

        [Test]
        public async Task DeleteOfferRemovesAnOfferFromContext()
        {
            OfferAddNEditViewModel offerAddNEditViewModel = new OfferAddNEditViewModel()
            {
                Name = "n",
                Description = "nccccccccccccccc",
                CategoryId = 1,
                CompanyId = null,
                DueDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Payment = 1
            };

            await companyService.AddOffer(offerAddNEditViewModel, "221ac3a1-5502-45b6-b328-8c47c142341r");

            Assert.That(context.Offers.ToArray().Count(), Is.EqualTo(1));

            await companyService.DeleteOffer(1);
            Assert.That(context.Offers.ToArray().Count(), Is.EqualTo(0));
        }
        
        [Test]
        public async Task DeleteTruckOfferRemovesAnOfferFromContext()
        {
            var truck = new TruckViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                HasParkTronic = false,
                HasBluetooth = false,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasSpeakers = false,
                RegistrationNumber = "QQ 1234 EE",
                ClassId = 2,
                Colour = "red",
                CompanyId = 2,
                Name = "fkiojrwh"
            };
            await companyService.RegisterTruck(truck, "221ac3a1-5502-45b6-b328-8c47c142341r");

            var offer = new TruckOfferAddNEditViewModel()
            {
                Description = "ghaklehfuiozahjfewn",
                Cost = 10000,
                Name = "TruckOffertichka",
                CompanyId = 2,
                TruckId = 1,
            };
            await companyService.AddTruckOffer(offer, "221ac3a1-5502-45b6-b328-8c47c142341r");

            Assert.That(context.TruckOffers.ToArray().Count(), Is.EqualTo(1));

            await companyService.DeleteTruckOffer(1);
            Assert.That(context.TruckOffers.ToArray().Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task ChangeOfferVisibilityChangesItsVisibility()
        {
            OfferAddNEditViewModel offerAddNEditViewModel = new OfferAddNEditViewModel()
            {
                Name = "n",
                Description = "nccccccccccccccc",
                CategoryId = 1,
                CompanyId = null,
                DueDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                Payment = 1
            };

            await companyService.AddOffer(offerAddNEditViewModel, "221ac3a1-5502-45b6-b328-8c47c142341d");

            Assert.That(context.Offers.ToArray().Count() == 1 && !context.Offers.First().IsApproved);

            await companyService.ChangeOfferVisibility(1);
            Assert.That(context.Offers.ToArray().Count() == 1 && context.Offers.First().IsApproved);
        }

        [Test]
        public async Task ChangeTruckOfferVisibilityChangesItsVisibility()
        {
            var truck = new TruckViewModel()
            {
                Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcShvf7LU6wTCi1NPcL1bDOz0mz8eBTxL9j1lt0rnpos&s",
                HasParkTronic = false,
                HasBluetooth = false,
                HasCDPlayer = false,
                HasInstaBrakes = false,
                HasSpeakers = false,
                RegistrationNumber = "QQ 1234 EE",
                ClassId = 2,
                Colour = "red",
                CompanyId = 2,
                Name = "fkiojrwh"
            };
            await companyService.RegisterTruck(truck, "221ac3a1-5502-45b6-b328-8c47c142341r");

            var offer = new TruckOfferAddNEditViewModel()
            {
                Description = "ghaklehfuiozahjfewn",
                Cost = 10000,
                Name = "TruckOffertichka",
                CompanyId = 2,
                TruckId = 1,
            };
            await companyService.AddTruckOffer(offer, "221ac3a1-5502-45b6-b328-8c47c142341r");
            var trOffer = await companyService.GetTruckOfferById(1);
            Assert.That(context.TruckOffers.Count() == 1 && !trOffer.IsApproved);

            await companyService.ChangeTruckOfferVisibility(1);
            Assert.That(context.TruckOffers.Count() == 1 && trOffer.IsApproved);
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}