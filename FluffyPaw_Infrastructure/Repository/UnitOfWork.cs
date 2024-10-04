using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private MyDbContext _context = new MyDbContext();
        private IGenericRepository<Account> _accountRepository;
        private IGenericRepository<BehaviorCategory> _behaviorCategoryRepository;
        private IGenericRepository<Booking> _bookingRepository;
        private IGenericRepository<BookingRating> _bookingRatingRepository;
        private IGenericRepository<Certificate> _certificateRepository;
        private IGenericRepository<Conversation> _conversationRepository;
        private IGenericRepository<ConversationMessage> _conversationMessageRepository;
        private IGenericRepository<Files> _filesRepository;
        private IGenericRepository<Identification> _identificationRepository;
        private IGenericRepository<MessageFile> _messageFileRepository;
        private IGenericRepository<Notification> _notificationRepository;
        private IGenericRepository<Pet> _petRepository;
        private IGenericRepository<PetCategory> _petCategoryRepository;
        private IGenericRepository<PetOwner> _petOwnerRepository;
        private IGenericRepository<PetType> _petTypeRepository;
        private IGenericRepository<Service> _serviceRepository;
        private IGenericRepository<ServiceType> _serviceTypeRepository;
        private IGenericRepository<Store> _storeRepository;
        private IGenericRepository<StoreFile> _storeFileRepository;
        private IGenericRepository<StoreService> _StoreServiceRepository;
        private IGenericRepository<Brand> _brandRepository;
        private IGenericRepository<Tracking> _trackingRepository;
        private IGenericRepository<TrackingFile> _trackingFileRepository;
        private IGenericRepository<VaccineHistory> _vaccineHistoryRepository;
        private IGenericRepository<Voucher> _voucherRepository;
        private IGenericRepository<Wallet> _walletRepository;

        public UnitOfWork()
        {

        }

        public IGenericRepository<Account> AccountRepository
        {
            get
            {

                if (_accountRepository == null)
                {
                    _accountRepository = new GenericRepository<Account>(_context);
                }
                return _accountRepository;
            }
        }

        public IGenericRepository<BehaviorCategory> BehaviorCategoryRepository
        {
            get
            {

                if (_behaviorCategoryRepository == null)
                {
                    _behaviorCategoryRepository = new GenericRepository<BehaviorCategory>(_context);
                }
                return _behaviorCategoryRepository;
            }
        }

        public IGenericRepository<Booking> BookingRepository
        {
            get
            {

                if (_bookingRepository == null)
                {
                    _bookingRepository = new GenericRepository<Booking>(_context);
                }
                return _bookingRepository;
            }
        }

        public IGenericRepository<BookingRating> BookingRatingRepository
        {
            get
            {

                if (_bookingRatingRepository == null)
                {
                    _bookingRatingRepository = new GenericRepository<BookingRating>(_context);
                }
                return _bookingRatingRepository;
            }
        }

        public IGenericRepository<Certificate> CertificateRepository
        {
            get
            {

                if (_certificateRepository == null)
                {
                    _certificateRepository = new GenericRepository<Certificate>(_context);
                }
                return _certificateRepository;
            }
        }

        public IGenericRepository<Conversation> ConversationRepository
        {
            get
            {

                if (_conversationRepository == null)
                {
                    _conversationRepository = new GenericRepository<Conversation>(_context);
                }
                return _conversationRepository;
            }
        }

        public IGenericRepository<ConversationMessage> ConversationMessageRepository
        {
            get
            {

                if (_conversationMessageRepository == null)
                {
                    _conversationMessageRepository = new GenericRepository<ConversationMessage>(_context);
                }
                return _conversationMessageRepository;
            }
        }

        public IGenericRepository<Files> FilesRepository
        {
            get
            {

                if (_filesRepository == null)
                {
                    _filesRepository = new GenericRepository<Files>(_context);
                }
                return _filesRepository;
            }
        }

        public IGenericRepository<Identification> IdentificationRepository
        {
            get
            {

                if (_identificationRepository == null)
                {
                    _identificationRepository = new GenericRepository<Identification>(_context);
                }
                return _identificationRepository;
            }
        }

        public IGenericRepository<MessageFile> MessageFileRepository
        {
            get
            {

                if (_messageFileRepository == null)
                {
                    _messageFileRepository = new GenericRepository<MessageFile>(_context);
                }
                return _messageFileRepository;
            }
        }

        public IGenericRepository<Notification> NotificationRepository
        {
            get
            {

                if (_notificationRepository == null)
                {
                    _notificationRepository = new GenericRepository<Notification>(_context);
                }
                return _notificationRepository;
            }
        }

        public IGenericRepository<Pet> PetRepository
        {
            get
            {

                if (_petRepository == null)
                {
                    _petRepository = new GenericRepository<Pet>(_context);
                }
                return _petRepository;
            }
        }

        public IGenericRepository<PetCategory> PetCategoryRepository
        {
            get
            {

                if (_petCategoryRepository == null)
                {
                    _petCategoryRepository = new GenericRepository<PetCategory>(_context);
                }
                return _petCategoryRepository;
            }
        }

        public IGenericRepository<PetOwner> PetOwnerRepository
        {
            get
            {

                if (_petOwnerRepository == null)
                {
                    _petOwnerRepository = new GenericRepository<PetOwner>(_context);
                }
                return _petOwnerRepository;
            }
        }

        public IGenericRepository<PetType> PetTypeRepository
        {
            get
            {

                if (_petTypeRepository == null)
                {
                    _petTypeRepository = new GenericRepository<PetType>(_context);
                }
                return _petTypeRepository;
            }
        }

        public IGenericRepository<Service> ServiceRepository
        {
            get
            {

                if (_serviceRepository == null)
                {
                    _serviceRepository = new GenericRepository<Service>(_context);
                }
                return _serviceRepository;
            }
        }

        public IGenericRepository<ServiceType> ServiceTypeRepository
        {
            get
            {

                if (_serviceTypeRepository == null)
                {
                    _serviceTypeRepository = new GenericRepository<ServiceType>(_context);
                }
                return _serviceTypeRepository;
            }
        }

        public IGenericRepository<Store> StoreRepository
        {
            get
            {

                if (_storeRepository == null)
                {
                    _storeRepository = new GenericRepository<Store>(_context);
                }
                return _storeRepository;
            }
        }

        public IGenericRepository<StoreFile> StoreFileRepository
        {
            get
            {

                if (_storeFileRepository == null)
                {
                    _storeFileRepository = new GenericRepository<StoreFile>(_context);
                }
                return _storeFileRepository;
            }
        }

        public IGenericRepository<StoreService> StoreServiceRepository
        {
            get
            {

                if (_StoreServiceRepository == null)
                {
                    _StoreServiceRepository = new GenericRepository<StoreService>(_context);
                }
                return _StoreServiceRepository;
            }
        }

        public IGenericRepository<Brand> BrandRepository
        {
            get
            {

                if (_brandRepository == null)
                {
                    _brandRepository = new GenericRepository<Brand>(_context);
                }
                return _brandRepository;
            }
        }

        public IGenericRepository<Tracking> TrackingRepository
        {
            get
            {

                if (_trackingRepository == null)
                {
                    _trackingRepository = new GenericRepository<Tracking>(_context);
                }
                return _trackingRepository;
            }
        }

        public IGenericRepository<TrackingFile> TrackingFileRepository
        {
            get
            {

                if (_trackingFileRepository == null)
                {
                    _trackingFileRepository = new GenericRepository<TrackingFile>(_context);
                }
                return _trackingFileRepository;
            }
        }

        public IGenericRepository<VaccineHistory> VaccineHistoryRepository
        {
            get
            {

                if (_vaccineHistoryRepository == null)
                {
                    _vaccineHistoryRepository = new GenericRepository<VaccineHistory>(_context);
                }
                return _vaccineHistoryRepository;
            }
        }

        public IGenericRepository<Voucher> VoucherRepository
        {
            get
            {

                if (_voucherRepository == null)
                {
                    _voucherRepository = new GenericRepository<Voucher>(_context);
                }
                return _voucherRepository;
            }
        }

        public IGenericRepository<Wallet> WalletRepository
        {
            get
            {

                if (_walletRepository == null)
                {
                    _walletRepository = new GenericRepository<Wallet>(_context);
                }
                return _walletRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
