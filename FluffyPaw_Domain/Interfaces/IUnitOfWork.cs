using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Account> AccountRepository { get; }
        public IGenericRepository<BehaviorCategory> BehaviorCategoryRepository { get; }
        public IGenericRepository<Booking> BookingRepository { get; }
        public IGenericRepository<BookingRating> BookingRatingRepository { get; }
        public IGenericRepository<Certificate> CertificateRepository { get; }
        public IGenericRepository<CertificateService> CertificateServiceRepository { get; }
        public IGenericRepository<Conversation> ConversationRepository { get; }
        public IGenericRepository<ConversationMessage> ConversationMessageRepository { get; }
        public IGenericRepository<Files> FilesRepository { get; }
        public IGenericRepository<MessageFile> MessageFileRepository { get; }
        public IGenericRepository<Notification> NotificationRepository { get; }
        public IGenericRepository<Pet> PetRepository { get; }
        public IGenericRepository<PetCategory> PetCategoryRepository { get; }
        public IGenericRepository<PetOwner> PetOwnerRepository { get; }
        public IGenericRepository<PetType> PetTypeRepository { get; }
        public IGenericRepository<Service> ServiceRepository { get; }
        public IGenericRepository<ServiceFile> ServiceFileRepository { get; }
        public IGenericRepository<ServiceType> ServiceTypeRepository { get; }
        public IGenericRepository<StaffAddress> StaffAddressRepository { get; }
        public IGenericRepository<StaffAddressFile> StaffAddressFileRepository { get; }
        public IGenericRepository<StaffAddressService> StaffAddressServiceRepository { get; }
        public IGenericRepository<StoreManager> StoreManagerRepository { get; }
        public IGenericRepository<Tracking> TrackingRepository { get; }
        public IGenericRepository<TrackingFile> TrackingFileRepository { get; }
        public IGenericRepository<VaccineHistory> VaccineHistoryRepository { get; }
        public IGenericRepository<Voucher> VoucherRepository { get; }
        public IGenericRepository<Wallet> WalletRepository { get; }
        void Save();
        Task SaveAsync();
        void Dispose();
    }
}
