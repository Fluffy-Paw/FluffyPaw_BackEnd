using FluffyPaw_Domain.Entities;
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
        public DbSet<CertificateService> CertificateServices { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ConversationMessage> ConversationMessages { get; set; }
        public DbSet<Files> Files { get; set; }
        public DbSet<MessageFile> MessageFiles { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetCategory> PetCategories { get; set; }
        public DbSet<PetOwner> PetOwners { get; set; }
        public DbSet<PetType> PetTypes { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceFile> ServiceFiles { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<StaffAddress> StaffAddresses { get; set; }
        public DbSet<StaffAddressFile> StaffAddressFiles { get; set; }
        public DbSet<StaffAddressService> StaffAddressServices { get; set; }
        public DbSet<StoreManager> StoreManagers { get; set; }
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
                new Account { Id = 1, Username = "FluffyPaw", Password = "4CC311E68571B9DB7EE9811B2D0215C97B48824469D3BF110875C97F63A90071CE2358E142222190D91A1D7C5E7DA6E4816052D5DF41B050CA01C7112BB48176", RoleName = "Admin", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow, Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = true },
                new Account { Id = 2, Username = "test1", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "StoreManager", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow, Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = true },
                new Account { Id = 3, Username = "test2", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "StoreManager", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow, Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = true },
                new Account { Id = 4, Username = "test3", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "Staff", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow, Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = true },
                new Account { Id = 5, Username = "test4", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "Staff", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow, Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = true },
                new Account { Id = 6, Username = "test5", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "PetOwner", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow, Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = true },
                new Account { Id = 7, Username = "test6", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "PetOwner", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow, Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = true }
                );

            modelBuilder.Entity<Wallet>().HasData(
                new Wallet { Id = 1, AccountId = 1, Balance = 1000000 },
                new Wallet { Id = 2, AccountId = 2, Balance = 0 },
                new Wallet { Id = 3, AccountId = 3, Balance = 0 },
                new Wallet { Id = 4, AccountId = 6, Balance = 100 },
                new Wallet { Id = 5, AccountId = 7, Balance = 100 }
                );

            modelBuilder.Entity<BehaviorCategory>().HasData(
                new BehaviorCategory { Id = 1, Name = "Chạy vòng tròn trước khi nằm xuống", Status = true},
                new BehaviorCategory { Id = 2, Name = "Liếm mặt chủ", Status = true },
                new BehaviorCategory { Id = 3, Name = "Rung lắc đuôi khi vui mừng", Status = true },
                new BehaviorCategory { Id = 4, Name = "Gầm gừ khi cảm thấy bị đe dọa", Status = true },
                new BehaviorCategory { Id = 5, Name = "Cào móng để đánh dấu lãnh thổ", Status = true }
                );

            modelBuilder.Entity<PetOwner>().HasData(
                new PetOwner { Id = 1, AccountId = 6, FullName = "Test", Gender = "Male", Dob = CoreHelper.SystemTimeNow, Phone = "1234567890", Address = "test", Status = "Tốt" },
                new PetOwner { Id = 2, AccountId = 7, FullName = "Test", Gender = "Male", Dob = CoreHelper.SystemTimeNow, Phone = "0123456789", Address = "test", Status = "Tốt" }
                );

            modelBuilder.Entity<PetCategory>().HasData(
                new PetCategory { Id = 1, Name = "Chó" },
                new PetCategory { Id = 2, Name = "Mèo" }
                );

            modelBuilder.Entity<PetType>().HasData(
                new PetType { Id = 1, Name = "Chó Phú Quốc", Image = "None", Status = true },
                new PetType { Id = 2, Name = "Mèo Tam Thể", Image = "None", Status = true }
                );

            modelBuilder.Entity<Pet>().HasData(
                new Pet { Id = 1, PetOwnerId = 1, PetCategoryId = 1, PetTypeId = 1, BehaviorCategoryId = 1, Name = "LuLu", Sex = "Male", Weight = 6.5F, Dob = DateTime.Parse("2022-08-23"), Allergy = "None", MicrochipNumber = "None", Decription = "test", IsNeuter = true, Status = "Available" },
                new Pet { Id = 2, PetOwnerId = 1, PetCategoryId = 2, PetTypeId = 1, BehaviorCategoryId = 2, Name = "MeowMeow", Sex = "FeMale", Weight = 5F, Dob = DateTime.Parse("2022-10-23"), Allergy = "None", MicrochipNumber = "None", Decription = "test1", IsNeuter = false, Status = "Unavailable" }
                );

            modelBuilder.Entity<Certificate>().HasData(
                new Certificate { Id = 1, Name = "Certificate of Excellence in Pet Grooming", File = "test", Description = "None" },
                new Certificate { Id = 2, Name = "Certificate of Excellence in Pet Grooming", File = "test", Description = "None" }
                );

            modelBuilder.Entity<ServiceType>().HasData(
                new ServiceType { Id = 1, Name = "Service Booking" },
                new ServiceType { Id = 2, Name = "Service Reservation" }
                );

            modelBuilder.Entity<StoreManager>().HasData(
                new StoreManager { Id = 1, AccountId = 2, Name = "StoreA", BusinessLicense = "None", Logo = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRsGufmy584u5_GDdLQaFiguxn8Qc5ILIZ7yA&s", Status = true },
                new StoreManager { Id = 2, AccountId = 3, Name = "StoreB", BusinessLicense = "None", Logo = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSxTepBxTlZftnBKdB6N4gQdZLF0W8ISlHdkA&s", Status = true }
                );

            modelBuilder.Entity<Service>().HasData(
                new Service { Id = 1, ServiceTypeId = 1, StoreManagerId = 1, Name = "Grooming", Duration = TimeSpan.FromMinutes(30), Cost = 100000, Description = "test", Status = true },
                new Service { Id = 2, ServiceTypeId = 2, StoreManagerId = 1, Name = "Vaccine", Duration = TimeSpan.FromMinutes(60), Cost = 200000, Description = "test", Status = true },
                new Service { Id = 3, ServiceTypeId = 1, StoreManagerId = 1, Name = "Hotel", Duration = TimeSpan.Zero, Cost = 100000, Description = "test", Status = true },
                new Service { Id = 4, ServiceTypeId = 2, StoreManagerId = 1, Name = "Training", Duration = TimeSpan.FromHours(1.5), Cost = 500000, Description = "test", Status = true }
                );

            modelBuilder.Entity<CertificateService>().HasData(
                new CertificateService { Id = 1, CertificateId = 1, ServiceId = 1 },
                new CertificateService { Id = 2, CertificateId = 2, ServiceId = 4 }
                );

            modelBuilder.Entity<StaffAddress>().HasData(
                new StaffAddress { Id = 1, AccountId = 6, StoreManagerId = 1, Address = "aaa", StaffAddressName = "Name" , Phone = "0192837465", TotalRating = 5.0f},
                new StaffAddress { Id = 2, AccountId = 7, StoreManagerId = 2, Address = "aaa", StaffAddressName = "Name" , Phone = "0192837465", TotalRating = 5.0f}
                );

            modelBuilder.Entity<StaffAddressService>().HasData(
                new StaffAddressService { Id = 1, StaffAddressId = 1, ServiceId = 1, StartTime = CoreHelper.SystemTimeNow, LimitPetOwner = 5, CurrentPetOwner = 0, TotalRating = 5.0F, Status = "Aceepted" },
                new StaffAddressService { Id = 2, StaffAddressId = 2, ServiceId = 2, StartTime = CoreHelper.SystemTimeNow, LimitPetOwner = 10, CurrentPetOwner = 0, TotalRating = 5.0F, Status = "Aceepted" }
                );

            modelBuilder.Entity<Booking>().HasData(
                new Booking { Id = 1, PetId = 1, StaffAddressServiceId = 1, PaymentMethod = "PayOS", Cost = 100000, Description = "test", CreateDate = CoreHelper.SystemTimeNow, StartTime = CoreHelper.SystemTimeNow, EndTime = CoreHelper.SystemTimeNow, Status = "Accept" }
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
