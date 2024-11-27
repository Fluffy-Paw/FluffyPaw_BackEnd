using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Enums;
using FluffyPaw_Domain.Utils;
using FluffyPaw_Repository.Enum;
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
        public DbSet<BillingRecord> billingRecords { get; set; }
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
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportCategory> ReportCategories { get; set; }
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

        public DbSet<Transaction> Transactions { get; set; }

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
                new Account { Id = 1, Username = "FluffyPaw", Password = "4CC311E68571B9DB7EE9811B2D0215C97B48824469D3BF110875C97F63A90071CE2358E142222190D91A1D7C5E7DA6E4816052D5DF41B050CA01C7112BB48176", RoleName = "Admin", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow.AddHours(7), Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = (int)AccountStatus.Active },
                new Account { Id = 2, Username = "test1", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "StoreManager", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow.AddHours(7), Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = (int)AccountStatus.Active },
                new Account { Id = 3, Username = "test2", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "StoreManager", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow.AddHours(7), Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = (int)AccountStatus.Active },
                new Account { Id = 4, Username = "test3", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "Staff", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow.AddHours(7), Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = (int)AccountStatus.Active },
                new Account { Id = 5, Username = "test4", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "Staff", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow.AddHours(7), Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = (int)AccountStatus.Active },
                new Account { Id = 6, Username = "test5", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "PetOwner", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow.AddHours(7), Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = (int)AccountStatus.Active },
                new Account { Id = 7, Username = "test6", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "PetOwner", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow.AddHours(7), Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = (int)AccountStatus.Active },
                new Account { Id = 8, Username = "test7", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "Staff", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow.AddHours(7), Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = (int)AccountStatus.Active },
                new Account { Id = 9, Username = "test8", Password = "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", RoleName = "Staff", Email = "test@gmail.com", CreateDate = CoreHelper.SystemTimeNow.AddHours(7), Avatar = "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", Status = (int)AccountStatus.Active }
                );

            modelBuilder.Entity<Wallet>().HasData(
                new Wallet { Id = 1, AccountId = 1, Balance = 1000000 },
                new Wallet { Id = 2, AccountId = 2, Balance = 0 },
                new Wallet { Id = 3, AccountId = 3, Balance = 0 },
                new Wallet { Id = 4, AccountId = 6, Balance = 100000000000 },
                new Wallet { Id = 5, AccountId = 7, Balance = 100000000000 }
                );

            modelBuilder.Entity<BehaviorCategory>().HasData(
                new BehaviorCategory { Id = 1, Name = "Không" },
                new BehaviorCategory { Id = 2, Name = "Chạy vòng tròn trước khi nằm xuống"},
                new BehaviorCategory { Id = 3, Name = "Liếm mặt chủ" },
                new BehaviorCategory { Id = 4, Name = "Rung lắc đuôi khi vui mừng"},
                new BehaviorCategory { Id = 5, Name = "Gầm gừ khi cảm thấy bị đe dọa" },
                new BehaviorCategory { Id = 6, Name = "Cào móng để đánh dấu lãnh thổ" }
                );

            modelBuilder.Entity<ReportCategory>().HasData(
                new ReportCategory { Id = 1, Type = "General", Name = "Tên đặt nhạy cảm" },
                new ReportCategory { Id = 2, Type = RoleName.Staff.ToString(), Name = "Hủy book quá nhiều lần cho một dịch vụ - Book xong hủy liên tục" },
                new ReportCategory { Id = 3, Type = RoleName.Staff.ToString(), Name = "Report sai thông tin" },
                new ReportCategory { Id = 4, Type = RoleName.Staff.ToString(), Name = "Nội dung phản cảm, Hình ảnh & ngôn từ nhạy cảm" },
                new ReportCategory { Id = 5, Type = RoleName.Staff.ToString(), Name = "Thông tin sai sự thật, lừa đảo" },
                new ReportCategory { Id = 6, Type = RoleName.Staff.ToString(), Name = "Nghi vấn buôn bán động vật trái phép" },
                new ReportCategory { Id = 7, Type = RoleName.Staff.ToString(), Name = "Khác" },
                new ReportCategory { Id = 8, Type = RoleName.PetOwner.ToString(), Name = "Dịch bị cấm buôn bán (nhằm mục đích trao đổi vật quý hiếm, hoang dã, 18+,....)" },
                new ReportCategory { Id = 9, Type = RoleName.PetOwner.ToString(), Name = "Dịch vụ có dấu hiệu lừa đảo" },
                new ReportCategory { Id = 10, Type = RoleName.PetOwner.ToString(), Name = "Dịch vụ gây ảnh hưởng/ tác động tiêu cực đến người dùng hoặc thú cưng" },
                new ReportCategory { Id = 11, Type = RoleName.PetOwner.ToString(), Name = "Hình ảnh không rõ ràng, sai sự thật, phản cảm,...." },
                new ReportCategory { Id = 12, Type = RoleName.PetOwner.ToString(), Name = "Dịch vụ có dấu hiệu tăng đơn ảo." },
                new ReportCategory { Id = 13, Type = RoleName.PetOwner.ToString(), Name = "Khác" }
                );

            modelBuilder.Entity<Report>().HasData(
                new Report { Id = 1, SenderId = 7, TargetId = 4, ReportCategoryId = 9, CreateDate = CoreHelper.SystemTimeNow, Description = "None", Status = true}
                );

            modelBuilder.Entity<PetOwner>().HasData(
                new PetOwner { Id = 1, AccountId = 6, FullName = "Test", Gender = "Male", Dob = CoreHelper.SystemTimeNow.AddHours(7), Phone = "1234567890", Address = "243/5 Đ. Nguyễn Tri Phương, Chánh Nghĩa, Thủ Dầu Một, Bình Dương, Việt Nam", Reputation = "Good" },
                new PetOwner { Id = 2, AccountId = 7, FullName = "Test", Gender = "Male", Dob = CoreHelper.SystemTimeNow.AddHours(7), Phone = "0123456789", Address = "243/5 Đ. Nguyễn Tri Phương, Chánh Nghĩa, Thủ Dầu Một, Bình Dương, Việt Nam", Reputation = "Good" }
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
                new Pet { Id = 2, PetOwnerId = 2, PetTypeId = 18, BehaviorCategoryId = 2, Name = "MeowMeow", Sex = "Female", Weight = 5F, Dob = DateTime.Parse("2022-10-23"), Allergy = "none", MicrochipNumber = "none", Decription = "test1", IsNeuter = false, Status = "Available" }
                );

            modelBuilder.Entity<VaccineHistory>().HasData(
                new VaccineHistory { Id = 1, PetId = 1, Image = "none", Name = "Vaccine 1", PetCurrentWeight = 4, VaccineDate = DateTime.Parse("2024-10-08"), NextVaccineDate = DateTime.Parse("2024-10-13"), Description = "Vaccine test", Status = VaccineStatus.Incomplete.ToString()},
                new VaccineHistory { Id = 2, PetId = 2, Image = "none", Name = "Vaccine 2", PetCurrentWeight = 4, VaccineDate = DateTime.Parse("2024-09-09"), Description = "Vaccine test", Status = VaccineStatus.Complete.ToString() }
                );

            modelBuilder.Entity<ServiceType>().HasData(
                new ServiceType { Id = 1, Name = "Grooming"},
                new ServiceType { Id = 2, Name = "Vaccine" },
                new ServiceType { Id = 3, Name = "Hotel" }
                );

            modelBuilder.Entity<Brand>().HasData(
                new Brand { Id = 1, AccountId = 2, Name = "BrandA", BrandEmail = "test1@gmail.com", BusinessLicense = "https://gray-wnem-prod.gtv-cdn.com/resizer/v2/ZRIYMJKRXFG4NGEORU4Z7MVE4U.png?auth=ca7b7f352a656d265f22b46ca0a9b36c6ecdb78546fc48e2cb1f260980998bd4&width=980&height=690&smart=true", Hotline = "0123456789", Logo = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRsGufmy584u5_GDdLQaFiguxn8Qc5ILIZ7yA&s", MST = "AAAAAAAAAAAA", Address = "A đường AA tổ AAA", Status = true },
                new Brand { Id = 2, AccountId = 3, Name = "BrandB", BrandEmail = "test1@gmail.com", BusinessLicense = "https://homeontherangepetsit.com/____impro/1/onewebmedia/2023%20Business%20License.jpg?etag=%22133c38-63fb9a14%22&sourceContentType=image%2Fjpeg&ignoreAspectRatio&resize=2000,1256&quality=85", Hotline = "0123456788", Logo = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSxTepBxTlZftnBKdB6N4gQdZLF0W8ISlHdkA&s", MST = "BBBBBBBBBBBB", Address = "B đường BB tổ BBB", Status = true }
                );

            modelBuilder.Entity<Store>().HasData(
                new Store { Id = 1, BrandId = 1, AccountId = 4, Name = "Chi nhánh A", OperatingLicense = "https://homeontherangepetsit.com/____impro/1/onewebmedia/2023%20Business%20License.jpg?etag=%22133c38-63fb9a14%22&sourceContentType=image%2Fjpeg&ignoreAspectRatio&resize=2000,1256&quality=85", Address = "157a Chòm Sao, Hưng Định, Thuận An, Bình Dương 098300, Việt Nam", Phone = "0123456789", TotalRating = 5f, Status = true},
                new Store { Id = 2, BrandId = 1, AccountId = 5, Name = "Chi nhánh B", OperatingLicense = "https://homeontherangepetsit.com/____impro/1/onewebmedia/2023%20Business%20License.jpg?etag=%22133c38-63fb9a14%22&sourceContentType=image%2Fjpeg&ignoreAspectRatio&resize=2000,1256&quality=85", Address = "157a Chòm Sao, Hưng Định, Thuận An, Bình Dương 098300, Việt Nam", Phone = "0123456789", TotalRating = 4f, Status = true},
                new Store { Id = 3, BrandId = 2, AccountId = 8, Name = "Chi nhánh C", OperatingLicense = "https://homeontherangepetsit.com/____impro/1/onewebmedia/2023%20Business%20License.jpg?etag=%22133c38-63fb9a14%22&sourceContentType=image%2Fjpeg&ignoreAspectRatio&resize=2000,1256&quality=85", Address = "157a Chòm Sao, Hưng Định, Thuận An, Bình Dương 098300, Việt Nam", Phone = "0123456789", TotalRating = 3f, Status = true},
                new Store { Id = 4, BrandId = 2, AccountId = 9, Name = "Chi nhánh D", OperatingLicense = "https://homeontherangepetsit.com/____impro/1/onewebmedia/2023%20Business%20License.jpg?etag=%22133c38-63fb9a14%22&sourceContentType=image%2Fjpeg&ignoreAspectRatio&resize=2000,1256&quality=85", Address = "157a Chòm Sao, Hưng Định, Thuận An, Bình Dương 098300, Việt Nam", Phone = "0123456789", TotalRating = 3f, Status = true}
                );

            modelBuilder.Entity<Service>().HasData(
                new Service { Id = 1, ServiceTypeId = 1, BrandId = 1, Name = "Pet Grooming", Image = "https://phongkhamthuythithipet.com/wp-content/uploads/2024/07/dich-vu-cham-soc-lam-dep-cho-thu-cung.jpg", Duration = TimeSpan.FromMinutes(30), Cost = 100000, Description = "test", BookingCount = 1, TotalRating = 0, Status = true },
                new Service { Id = 2, ServiceTypeId = 2, BrandId = 1, Name = "Vaccine", Image = "https://hillcrestvets.co.za/wp-content/uploads/2020/10/Pet-Vaccinations.jpg", Duration = TimeSpan.FromMinutes(60), Cost = 200000, Description = "test", BookingCount = 0, TotalRating = 0, Status = true },
                new Service { Id = 3, ServiceTypeId = 3, BrandId = 2, Name = "Hotel for your Boss", Image = "https://bizweb.dktcdn.net/thumb/1024x1024/100/092/840/products/14b275e8-4ef4-4f5e-b5fb-c11243dbae1a.jpg?v=1677488701687", Duration = TimeSpan.FromHours(23), Cost = 100000, Description = "test", BookingCount = 0, TotalRating = 0, Status = true }
                );

            modelBuilder.Entity<Certificate>().HasData(
                new Certificate { Id = 1, ServiceId = 1, Name = "Certificate of Excellence in Pet Grooming", File = "test", Description = "none" },
                new Certificate { Id = 2, ServiceId = 1, Name = "Certificate of Excellence in Pet Grooming", File = "test", Description = "none" },
                new Certificate { Id = 3, ServiceId = 2, Name = "Certificate of Excellence in Pet Vaccine", File = "test", Description = "none" },
                new Certificate { Id = 4, ServiceId = 3, Name = "Certificate of Excellence in Pet Hotel", File = "test", Description = "none" },
                new Certificate { Id = 5, ServiceId = 3, Name = "Certificate of Excellence in Pet Hotel", File = "test", Description = "none" },
                new Certificate { Id = 6, ServiceId = 3, Name = "Certificate of Excellence in Pet Hotel", File = "test", Description = "none" }
                );

            modelBuilder.Entity<StoreService>().HasData(
                new StoreService { Id = 1, StoreId = 1, ServiceId = 1, StartTime = CoreHelper.SystemTimeNow.AddDays(10).AddHours(7), LimitPetOwner = 100, CurrentPetOwner = 0, Status = StoreServiceStatus.Available.ToString() },
                new StoreService { Id = 2, StoreId = 1, ServiceId = 1, StartTime = CoreHelper.SystemTimeNow.AddDays(10).AddHours(10), LimitPetOwner = 100, CurrentPetOwner = 0, Status = StoreServiceStatus.Available.ToString() },
                new StoreService { Id = 3, StoreId = 1, ServiceId = 1, StartTime = CoreHelper.SystemTimeNow.AddDays(10).AddHours(14), LimitPetOwner = 100, CurrentPetOwner = 0, Status = StoreServiceStatus.Available.ToString() },
                new StoreService { Id = 4, StoreId = 2, ServiceId = 1, StartTime = CoreHelper.SystemTimeNow.AddDays(10).AddHours(7), LimitPetOwner = 50, CurrentPetOwner = 0, Status = StoreServiceStatus.Available.ToString() },
                new StoreService { Id = 5, StoreId = 2, ServiceId = 1, StartTime = CoreHelper.SystemTimeNow.AddDays(10).AddHours(7), LimitPetOwner = 50, CurrentPetOwner = 0, Status = StoreServiceStatus.Available.ToString() },
                new StoreService { Id = 6, StoreId = 2, ServiceId = 1, StartTime = CoreHelper.SystemTimeNow.AddDays(10).AddHours(7), LimitPetOwner = 50, CurrentPetOwner = 0, Status = StoreServiceStatus.Available.ToString() },
                new StoreService { Id = 7, StoreId = 3, ServiceId = 2, StartTime = CoreHelper.SystemTimeNow.AddDays(10).AddHours(7), LimitPetOwner = 50, CurrentPetOwner = 0, Status = StoreServiceStatus.Available.ToString() },
                new StoreService { Id = 8, StoreId = 3, ServiceId = 2, StartTime = CoreHelper.SystemTimeNow.AddDays(10).AddHours(7), LimitPetOwner = 50, CurrentPetOwner = 0, Status = StoreServiceStatus.Available.ToString() },
                new StoreService { Id = 9, StoreId = 3, ServiceId = 2, StartTime = CoreHelper.SystemTimeNow.AddDays(10).AddHours(7), LimitPetOwner = 50, CurrentPetOwner = 0, Status = StoreServiceStatus.Available.ToString() },
                new StoreService { Id = 10, StoreId = 3, ServiceId = 3, StartTime = new DateTimeOffset(2024, 11, 25, 12, 0, 0, TimeSpan.Zero), CurrentPetOwner = 0, Status = StoreServiceStatus.Available.ToString() },
                new StoreService { Id = 11, StoreId = 3, ServiceId = 3, StartTime = new DateTimeOffset(2024, 11, 26, 12, 0, 0, TimeSpan.Zero), LimitPetOwner = 100, CurrentPetOwner = 0, Status = StoreServiceStatus.Available.ToString() },
                new StoreService { Id = 12, StoreId = 3, ServiceId = 3, StartTime = new DateTimeOffset(2024, 11, 27, 12, 0, 0, TimeSpan.Zero), LimitPetOwner = 100, CurrentPetOwner = 0, Status = StoreServiceStatus.Available.ToString() },
                new StoreService { Id = 13, StoreId = 3, ServiceId = 3, StartTime = new DateTimeOffset(2024, 11, 28, 12, 0, 0, TimeSpan.Zero), LimitPetOwner = 100, CurrentPetOwner = 0, Status = StoreServiceStatus.Available.ToString() },
                new StoreService { Id = 14, StoreId = 3, ServiceId = 3, StartTime = new DateTimeOffset(2024, 11, 29, 12, 0, 0, TimeSpan.Zero), LimitPetOwner = 100, CurrentPetOwner = 0, Status = StoreServiceStatus.Available.ToString() },
                new StoreService { Id = 15, StoreId = 3, ServiceId = 3, StartTime = new DateTimeOffset(2024, 11, 30, 12, 0, 0, TimeSpan.Zero), LimitPetOwner = 100, CurrentPetOwner = 0, Status = StoreServiceStatus.Available.ToString() },
                new StoreService { Id = 16, StoreId = 3, ServiceId = 3, StartTime = new DateTimeOffset(2024, 12, 01, 12, 0, 0, TimeSpan.Zero), LimitPetOwner = 100, CurrentPetOwner = 0, Status = StoreServiceStatus.Available.ToString() },
                new StoreService { Id = 17, StoreId = 4, ServiceId = 3, StartTime = new DateTimeOffset(2024, 11, 25, 12, 0, 0, TimeSpan.Zero), LimitPetOwner = 100, CurrentPetOwner = 0, Status = StoreServiceStatus.Available.ToString() },
                new StoreService { Id = 18, StoreId = 4, ServiceId = 3, StartTime = new DateTimeOffset(2024, 11, 26, 12, 0, 0, TimeSpan.Zero), LimitPetOwner = 100, CurrentPetOwner = 0, Status = StoreServiceStatus.Available.ToString() },
                new StoreService { Id = 19, StoreId = 4, ServiceId = 3, StartTime = new DateTimeOffset(2024, 11, 27, 12, 0, 0, TimeSpan.Zero), LimitPetOwner = 100, CurrentPetOwner = 0, Status = StoreServiceStatus.Available.ToString() },
                new StoreService { Id = 20, StoreId = 4, ServiceId = 3, StartTime = new DateTimeOffset(2024, 11, 28, 12, 0, 0, TimeSpan.Zero), LimitPetOwner = 100, CurrentPetOwner = 0, Status = StoreServiceStatus.Available.ToString() }
                );
        }
    }
}
