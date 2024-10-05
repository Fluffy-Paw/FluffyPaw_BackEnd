using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Enums;
using FluffyPaw_Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Infrastructure.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() { }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<BehaviorCategory> BehaviorCategories { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingRating> BookingRatings { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ConversationMessage> ConversationMessages { get; set; }
        public DbSet<Files> Files { get; set; }
        public DbSet<Identification> Identifications { get; set; }
        public DbSet<MessageFile> MessageFiles { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetCategory> PetCategories { get; set; }
        public DbSet<PetOwner> PetOwners { get; set; }
        public DbSet<PetType> PetTypes { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreFile> StoreFiles { get; set; }
        public DbSet<StoreService> StoreServices { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Tracking> Trackings { get; set; }
        public DbSet<TrackingFile> TrackingFiles { get; set; }
        public DbSet<VaccineHistory> VaccineHistories { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("MyDB");
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>().HasData(
                new Account { Id = 1, Username = "FluffyPaw", Password = "4CC311E68571B9DB7EE9811B2D0215C97B48824469D3BF110875C97F63A90071CE2358E142222190D91A1D7C5E7DA6E4816052D5DF41B050CA01C7112BB48176", RoleName = "Admin", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow, Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = (int)AccountStatus.Active },
                new Account { Id = 2, Username = "test1", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "StoreManager", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow, Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = (int)AccountStatus.Active },
                new Account { Id = 3, Username = "test2", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "StoreManager", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow, Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = (int)AccountStatus.Active },
                new Account { Id = 4, Username = "test3", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "Staff", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow, Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = (int)AccountStatus.Active },
                new Account { Id = 5, Username = "test4", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "Staff", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow, Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = (int)AccountStatus.Active },
                new Account { Id = 6, Username = "test5", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "PetOwner", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow, Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = (int)AccountStatus.Active },
                new Account { Id = 7, Username = "test6", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "PetOwner", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow, Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = (int)AccountStatus.Active },
                new Account { Id = 8, Username = "test7", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "Staff", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow, Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = (int)AccountStatus.Active },
                new Account { Id = 9, Username = "test8", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "Staff", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow, Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = (int)AccountStatus.Active }


                );

            modelBuilder.Entity<Wallet>().HasData(
                new Wallet { Id = 1, AccountId = 1, Balance = 1000000 },
                new Wallet { Id = 2, AccountId = 2, Balance = 0 },
                new Wallet { Id = 3, AccountId = 3, Balance = 0 },
                new Wallet { Id = 4, AccountId = 6, Balance = 100 },
                new Wallet { Id = 5, AccountId = 7, Balance = 100 }
                );

            modelBuilder.Entity<BehaviorCategory>().HasData(
                new BehaviorCategory { Id = 1, Name = "Chạy vòng tròn trước khi nằm xuống"},
                new BehaviorCategory { Id = 2, Name = "Liếm mặt chủ" },
                new BehaviorCategory { Id = 3, Name = "Rung lắc đuôi khi vui mừng"},
                new BehaviorCategory { Id = 4, Name = "Gầm gừ khi cảm thấy bị đe dọa" },
                new BehaviorCategory { Id = 5, Name = "Cào móng để đánh dấu lãnh thổ" }
                );

            modelBuilder.Entity<PetOwner>().HasData(
                new PetOwner { Id = 1, AccountId = 6, FullName = "Test", Gender = "Male", Dob = CoreHelper.SystemTimeNow, Phone = "1234567890", Address = "test", Reputation = "Good" },
                new PetOwner { Id = 2, AccountId = 7, FullName = "Test", Gender = "Male", Dob = CoreHelper.SystemTimeNow, Phone = "0123456789", Address = "test", Reputation = "Good" }
                );

            modelBuilder.Entity<PetCategory>().HasData(
                new PetCategory { Id = 1, Name = "Dog" },
                new PetCategory { Id = 2, Name = "Cat" }
                );

            modelBuilder.Entity<PetType>().HasData(
                new PetType { Id = 1, PetCategoryId = 1, Name = "Chó Chihuahua", Image = "none" },
                new PetType { Id = 2, PetCategoryId = 1, Name = "Chó Bắc Kinh", Image = "none" },
                new PetType { Id = 3, PetCategoryId = 1, Name = "Chó Bắc Kinh lai Nhật", Image = "none" },
                new PetType { Id = 4, PetCategoryId = 1, Name = "Chó Dachshund (Lạp Xưởng/Xúc Xích)", Image = "none" },
                new PetType { Id = 5, PetCategoryId = 1, Name = "Chó Phú Quốc", Image = "none" },
                new PetType { Id = 6, PetCategoryId = 1, Name = "Chó Poodle", Image = "none" },
                new PetType { Id = 7, PetCategoryId = 1, Name = "Chó Pug", Image = "none" },
                new PetType { Id = 8, PetCategoryId = 1, Name = "Chó Alaska", Image = "none" },
                new PetType { Id = 9, PetCategoryId = 1, Name = "Chó Husky", Image = "none" },
                new PetType { Id = 10, PetCategoryId = 1, Name = "Chó Samoyed", Image = "none" },
                new PetType { Id = 11, PetCategoryId = 1, Name = "Chó Pomeranian (Phốc sóc)", Image = "none" },
                new PetType { Id = 12, PetCategoryId = 1, Name = "Chó Beagle", Image = "none" },
                new PetType { Id = 13, PetCategoryId = 1, Name = "Chó Shiba Inu", Image = "none" },
                new PetType { Id = 14, PetCategoryId = 1, Name = "Chó Golden Retriever", Image = "none" },
                new PetType { Id = 15, PetCategoryId = 1, Name = "Chó Becgie", Image = "none" },
                new PetType { Id = 16, PetCategoryId = 1, Name = "Chó Corgi", Image = "none" },
                new PetType { Id = 17, PetCategoryId = 1, Name = "Chó Mông Cộc", Image = "none" },
                new PetType { Id = 18, PetCategoryId = 2, Name = "Mèo Xiêm", Image = "none" },
                new PetType { Id = 19, PetCategoryId = 2, Name = "Mèo Anh lông ngắn", Image = "none" },
                new PetType { Id = 20, PetCategoryId = 2, Name = "Mèo Anh lông dài", Image = "none" },
                new PetType { Id = 21, PetCategoryId = 2, Name = "Mèo Ai Cập", Image = "none" },
                new PetType { Id = 22, PetCategoryId = 2, Name = "Mèo Ba Tư", Image = "none" },
                new PetType { Id = 23, PetCategoryId = 2, Name = "Mèo Bali", Image = "none" },
                new PetType { Id = 24, PetCategoryId = 2, Name = "Mèo Bengal", Image = "none" },
                new PetType { Id = 25, PetCategoryId = 2, Name = "Mèo Scottish Fold", Image = "none" },
                new PetType { Id = 26, PetCategoryId = 2, Name = "Mèo Munchkin", Image = "none" },
                new PetType { Id = 27, PetCategoryId = 2, Name = "Mèo mướp", Image = "none" },
                new PetType { Id = 28, PetCategoryId = 2, Name = "Mèo Ragdoll", Image = "none" },
                new PetType { Id = 29, PetCategoryId = 2, Name = "Mèo Maine Coon", Image = "none" },
                new PetType { Id = 30, PetCategoryId = 2, Name = "Mèo Angora", Image = "none" },
                new PetType { Id = 31, PetCategoryId = 2, Name = "Mèo Laperm", Image = "none" },
                new PetType { Id = 32, PetCategoryId = 2, Name = "Mèo Somali", Image = "none" },
                new PetType { Id = 33, PetCategoryId = 2, Name = "Mèo Toyger", Image = "none" },
                new PetType { Id = 34, PetCategoryId = 2, Name = "Mèo Turkish Van", Image = "none" },
                new PetType { Id = 35, PetCategoryId = 2, Name = "Mèo Miến Điện", Image = "none" },
                new PetType { Id = 36, PetCategoryId = 2, Name = "Mèo Exotic", Image = "none" }
                );

            modelBuilder.Entity<Pet>().HasData(
                new Pet { Id = 1, PetOwnerId = 1, PetTypeId = 1, BehaviorCategoryId = 1, Name = "LuLu", Sex = "Male", Weight = 6.5F, Dob = DateTime.Parse("2022-08-23"), Allergy = "none", MicrochipNumber = "none", Decription = "test", IsNeuter = true, Status = "Available" },
                new Pet { Id = 2, PetOwnerId = 2, PetTypeId = 18, BehaviorCategoryId = 2, Name = "MeowMeow", Sex = "FeMale", Weight = 5F, Dob = DateTime.Parse("2022-10-23"), Allergy = "none", MicrochipNumber = "none", Decription = "test1", IsNeuter = false, Status = "Unavailable" }
                );

            modelBuilder.Entity<ServiceType>().HasData(
                new ServiceType { Id = 1, Name = "Grooming"},
                new ServiceType { Id = 2, Name = "Vaccine"}
                );

            modelBuilder.Entity<Brand>().HasData(
                new Brand { Id = 1, AccountId = 2, Name = "StoreA", BrandEmail = "test1@gmail.com", BusinessLicense = "none", Hotline = "0123456789", Logo = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRsGufmy584u5_GDdLQaFiguxn8Qc5ILIZ7yA&s", MST = "none", Address = "test", Status = true },
                new Brand { Id = 2, AccountId = 3, Name = "StoreB", BrandEmail = "test1@gmail.com", BusinessLicense = "none", Hotline = "0123456789", Logo = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSxTepBxTlZftnBKdB6N4gQdZLF0W8ISlHdkA&s", MST = "none", Address = "test", Status = true }
                );

            modelBuilder.Entity<Store>().HasData(
                new Store { Id = 1, BrandId = 1, AccountId = 4, Name = "Chi nhánh A", Address = "số AAA đường AA, Thành phố A", Phone = "0123456789", TotalRating = 0f, Status = true},
                new Store { Id = 2, BrandId = 1, AccountId = 5, Name = "Chi nhánh B", Address = "số BBB đường BB, Thành phố B", Phone = "0123456789", TotalRating = 0f, Status = true},
                new Store { Id = 3, BrandId = 2, AccountId = 8, Name = "Chi nhánh C", Address = "số CCC đường CC, Thành phố C", Phone = "0123456789", TotalRating = 0f, Status = true},
                new Store { Id = 4, BrandId = 2, AccountId = 9, Name = "Chi nhánh D", Address = "số DDD đường DD, Thành phố d", Phone = "0123456789", TotalRating = 0f, Status = true}
                );

            modelBuilder.Entity<Service>().HasData(
                new Service { Id = 1, ServiceTypeId = 1, BrandId = 1, Name = "Grooming", Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSxTepBxTlZftnBKdB6N4gQdZLF0W8ISlHdkA&s", Duration = TimeSpan.FromMinutes(30), Cost = 100000, Description = "test", BookingCount = 0, TotalRating = 0, Status = true },
                new Service { Id = 2, ServiceTypeId = 2, BrandId = 1, Name = "Vaccine", Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSxTepBxTlZftnBKdB6N4gQdZLF0W8ISlHdkA&s", Duration = TimeSpan.FromMinutes(60), Cost = 200000, Description = "test", BookingCount = 0, TotalRating = 0, Status = true },
                new Service { Id = 3, ServiceTypeId = 1, BrandId = 1, Name = "Hotel", Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSxTepBxTlZftnBKdB6N4gQdZLF0W8ISlHdkA&s", Duration = TimeSpan.Zero, Cost = 100000, Description = "test", BookingCount = 0, TotalRating = 0, Status = true },
                new Service { Id = 4, ServiceTypeId = 2, BrandId = 1, Name = "Training", Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSxTepBxTlZftnBKdB6N4gQdZLF0W8ISlHdkA&s", Duration = TimeSpan.FromHours(1.5), Cost = 500000, Description = "test", BookingCount = 0, TotalRating = 0, Status = true }
                );

            modelBuilder.Entity<Certificate>().HasData(
                new Certificate { Id = 1, ServiceId = 1, Name = "Certificate of Excellence in Pet Grooming", File = "test", Description = "none" },
                new Certificate { Id = 2, ServiceId = 1, Name = "Certificate of Excellence in Pet Grooming", File = "test", Description = "none" },
                new Certificate { Id = 3, ServiceId = 2, Name = "Certificate of Excellence in Pet Grooming", File = "test", Description = "none" },
                new Certificate { Id = 4, ServiceId = 3, Name = "Certificate of Excellence in Pet Grooming", File = "test", Description = "none" },
                new Certificate { Id = 5, ServiceId = 3, Name = "Certificate of Excellence in Pet Grooming", File = "test", Description = "none" },
                new Certificate { Id = 6, ServiceId = 3, Name = "Certificate of Excellence in Pet Grooming", File = "test", Description = "none" },
                new Certificate { Id = 7, ServiceId = 4, Name = "Certificate of Excellence in Pet Grooming", File = "test", Description = "none" }
                );

            modelBuilder.Entity<StoreService>().HasData(
                new StoreService { Id = 1, StoreId = 1, ServiceId = 1, StartTime = CoreHelper.SystemTimeNow, LimitPetOwner = 5, CurrentPetOwner = 0, Status = "Acepted" },
                new StoreService { Id = 2, StoreId = 2, ServiceId = 2, StartTime = CoreHelper.SystemTimeNow, LimitPetOwner = 10, CurrentPetOwner = 0, Status = "Acepted" }
                );

            modelBuilder.Entity<Booking>().HasData(
                new Booking { Id = 1, PetId = 1, StoreServiceId = 1, PaymentMethod = "PayOS", Cost = 100000, Description = "test", CreateDate = CoreHelper.SystemTimeNow, StartTime = CoreHelper.SystemTimeNow, EndTime = CoreHelper.SystemTimeNow, Status = "Accept" }
                );

            modelBuilder.Entity<BookingRating>().HasData(
                new BookingRating { Id = 1, BookingId = 1, PetOwnerId = 1, Description = "test", Status = true }
                );

            modelBuilder.Entity<Tracking>().HasData(
                new Tracking { Id = 1, BookingId = 1, UploadDate = DateTimeOffset.Now, Description = "test", Status = true }
                );
        }
    }
}
