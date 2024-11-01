﻿using AutoMapper;
using FluffyPaw_Application.DTO.Request.ConversationRequest;
using FluffyPaw_Application.DTO.Response.ConversationResponse;
using FluffyPaw_Application.DTO.Response.FilesResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Domain.Utils;
using FluffyPaw_Repository.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.ServiceImplements
{
    public class ConversationService : IConversationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthentication _authentication;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IJobScheduler _jobScheduler;
        private readonly IFirebaseConfiguration _firebaseConfiguration;
        private readonly INotificationService _notificationService;

        public ConversationService(IUnitOfWork unitOfWork, IMapper mapper, IAuthentication authentication,
                                IHttpContextAccessor contextAccessor, IJobScheduler jobScheduler,
                                IFirebaseConfiguration firebaseConfiguration, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authentication = authentication;
            _contextAccessor = contextAccessor;
            _jobScheduler = jobScheduler;
            _firebaseConfiguration = firebaseConfiguration;
            _notificationService = notificationService;
        }

        public async Task<List<ConversationResponse>> GetAllConversation()
        {
            var userId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy tài khoản của bạn.");
            }

            IEnumerable<Conversation> conversations;

            if (account.RoleName == RoleName.Staff.ToString())
            {
                conversations = _unitOfWork.ConversationRepository.Get(c => c.StaffAccountId == account.Id,
                                                    includeProperties: "ConversationMessages");
                
            }
            else if (account.RoleName == RoleName.PetOwner.ToString())
            {
                conversations = _unitOfWork.ConversationRepository.Get(c => c.PoAccountId == account.Id,
                                                    includeProperties: "ConversationMessages");
            }
            else
            {
                throw new CustomException.InvalidDataException("Vai trò không hợp lệ.");
            }

            if (!conversations.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy hội thoại nào.");
            }

            var conversationResponses = _mapper.Map<List<ConversationResponse>>(conversations);

            foreach (var conversationResponse in conversationResponses)
            {
                var conversation = conversations.First(c => c.Id == conversationResponse.Id);

                var lastMessage = conversation.ConversationMessages
                    .OrderByDescending(m => m.CreateTime)
                    .FirstOrDefault();
                conversationResponse.LastMessege = lastMessage?.Content ?? "Hãy nhắn tin để bắt đầu cuộc trò chuyện.";

                var petOwner = _unitOfWork.PetOwnerRepository.Get(po => po.AccountId == conversation.PoAccountId,
                                                includeProperties: "Account").FirstOrDefault();
                if (petOwner != null)
                {
                    conversationResponse.PoName = petOwner.FullName;
                    conversationResponse.PoAvatar = petOwner.Account.Avatar;
                }

                var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == conversation.StaffAccountId,
                                                includeProperties: "Brand").FirstOrDefault();
                if (store != null)
                {
                    conversationResponse.StoreName = store.Name;
                    conversationResponse.StoreAvatar = store.Brand.Logo;
                }
            }

            conversationResponses = conversationResponses
                .OrderByDescending(c => c.LastMessege)
                .ToList();

            return conversationResponses;
        }

        public async Task<ConversationResponse> CreateConversation(ConversationRequest conversationRequest)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy tài khoản của bạn.");
            }

            var existingConversation = _unitOfWork.ConversationRepository.Get(c =>
                                    (c.PoAccountId == conversationRequest.PersonId && c.StaffAccountId == account.Id) ||
                                    c.PoAccountId == account.Id && c.StaffAccountId == conversationRequest.PersonId).FirstOrDefault();
            if (existingConversation != null)
            {
                var existingResponse = _mapper.Map<ConversationResponse>(existingConversation);

                PopulateAdditionalFields(existingResponse, existingConversation);

                return existingResponse;
            }

            Conversation newConversation = null;

            if (account.RoleName == RoleName.Staff.ToString())
            {
                newConversation = new Conversation 
                {
                    PoAccountId = conversationRequest.PersonId,
                    StaffAccountId = account.Id,
                    LastMessege = "Hãy nhắn tin để bắt đầu cuộc trò chuyện.",
                    IsOpen = true
                };
            }
            else if (account.RoleName == RoleName.PetOwner.ToString())
            {
                newConversation = new Conversation
                {
                    PoAccountId = account.Id,
                    StaffAccountId = conversationRequest.PersonId,
                    LastMessege = "Hãy nhắn tin để bắt đầu cuộc trò chuyện.",
                    IsOpen = true
                };
            }

            _unitOfWork.ConversationRepository.Insert(newConversation);
            await _unitOfWork.SaveAsync();

            var conversationResponse = _mapper.Map<ConversationResponse>(newConversation);

            PopulateAdditionalFields(conversationResponse, newConversation);

            return conversationResponse;
        }

        public async Task<bool> OpenConversation(long id)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy tài khoản của bạn.");
            }

            var existingConversation = _unitOfWork.ConversationRepository.GetByID(id);
            if (existingConversation == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy hội thoại.");
            }

            if ((account.RoleName == RoleName.Staff.ToString() && existingConversation.StaffAccountId != account.Id) ||
                account.RoleName == RoleName.PetOwner.ToString() && existingConversation.PoAccountId != account.Id)
            {
                throw new CustomException.InvalidDataException("Bạn không có quyền thao tác trong cuộc trò chuyện này.");
            }

            if (existingConversation.CloseAccountId != account.Id)
            {
                throw new CustomException.InvalidDataException("Bạn đã bị chặn từ phía còn lại.");
            }

            existingConversation.IsOpen = true;

            _unitOfWork.ConversationRepository.Update(existingConversation);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<bool> CloseConversation(long id)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy tài khoản của bạn.");
            }

            var existingConversation = _unitOfWork.ConversationRepository.GetByID(id);
            if (existingConversation == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy hội thoại.");
            }

            if (existingConversation.IsOpen == false)
            {
                throw new CustomException.InvalidDataException("Cuội hội thoại này đã bị chặn trước đó.");
            }

            if ((account.RoleName == RoleName.Staff.ToString() && existingConversation.StaffAccountId != account.Id) ||
                account.RoleName == RoleName.PetOwner.ToString() && existingConversation.PoAccountId != account.Id)
            {
                throw new CustomException.InvalidDataException("Bạn không có quyền thao tác trong cuộc trò chuyện này.");
            }

            existingConversation.CloseAccountId = account.Id;
            existingConversation.IsOpen = false;

            _unitOfWork.ConversationRepository.Update(existingConversation);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteConversation(long id)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy tài khoản của bạn.");
            }

            var existingConversation = _unitOfWork.ConversationRepository.GetByID(id);
            if (existingConversation == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy hội thoại.");
            }

            if ((account.RoleName == RoleName.Staff.ToString() && existingConversation.StaffAccountId != account.Id) ||
                account.RoleName == RoleName.PetOwner.ToString() && existingConversation.PoAccountId != account.Id)
            {
                throw new CustomException.InvalidDataException("Bạn không có quyền thao tác trong cuộc trò chuyện này.");
            }

            _unitOfWork.ConversationRepository.Delete(existingConversation);
            _unitOfWork.Save();

            return true;
        }

        public async Task<List<ConversationMessageResponse>> GetAllConversationMessageByConversationId(long id)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy tài khoản của bạn.");
            }

            IEnumerable<ConversationMessage> conversationMessages;
            Conversation conversation;

            if (account.RoleName == RoleName.Staff.ToString())
            {
                conversation = _unitOfWork.ConversationRepository.Get(c => c.Id == id && c.StaffAccountId == account.Id
                                                    && c.IsOpen == true).FirstOrDefault();
            }
            else if (account.RoleName == RoleName.PetOwner.ToString())
            {
                conversation = _unitOfWork.ConversationRepository.Get(c => c.Id == id && c.PoAccountId == account.Id
                                                    && c.IsOpen == true).FirstOrDefault();
            }
            else
            {
                throw new CustomException.InvalidDataException("Vai trò không hợp lệ.");
            }

            if (conversation == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy cuộc trò chuyện hoặc bạn không có quyền truy cập.");
            }

            conversationMessages = _unitOfWork.ConversationMessageRepository.Get(
                cm => cm.ConversationId == conversation.Id,
                orderBy: cm => cm.OrderBy(m => m.CreateTime)
            );

            var conversationMessageResponses = _mapper.Map<List<ConversationMessageResponse>>(conversationMessages);
            return conversationMessageResponses;
        }

        public async Task<ConversationMessageResponse> SendMessage(ConversationMessageRequest conversationMessageRequest)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy tài khoản của bạn.");
            }

            var conversation = _unitOfWork.ConversationRepository.Get(c => c.Id == conversationMessageRequest.ConversationId).FirstOrDefault();
            if (conversation == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy cuộc trò chuyện.");
            }
            if (conversation.IsOpen == false)
            {
                throw new CustomException.InvalidDataException("Cuội hội thoại này đã bị chặn. Vui lòng gỡ để tiếp tục trò chuyện.");
            }

            if ((account.RoleName == RoleName.Staff.ToString() && conversation.StaffAccountId != account.Id) ||
                account.RoleName == RoleName.PetOwner.ToString() && conversation.PoAccountId != account.Id)
            {
                throw new CustomException.InvalidDataException("Người gửi không có quyền gửi tin nhắn trong cuộc trò chuyện này.");
            }

            var newMessage = new ConversationMessage
            {
                ConversationId = conversationMessageRequest.ConversationId,
                SenderId = account.Id,
                Content = conversationMessageRequest.Content,
                CreateTime = CoreHelper.SystemTimeNow,
                IsSeen = false,
                ReplyMessageId = conversationMessageRequest.ReplyMessageId,
            };

            _unitOfWork.ConversationMessageRepository.Insert(newMessage);
            await _unitOfWork.SaveAsync();

            var fileResponses = new List<FileResponse>();

            foreach (var file in conversationMessageRequest.Files)
            {
                var newFile = new Files
                {
                    File = await _firebaseConfiguration.UploadImage(file),
                    Status = true
                };
                _unitOfWork.FilesRepository.Insert(newFile);
                await _unitOfWork.SaveAsync();

                var newMessageFile = new MessageFile
                {
                    FileId = newFile.Id,
                    MessageId = newMessage.Id,
                };
                _unitOfWork.MessageFileRepository.Insert(newMessageFile);
                _unitOfWork.Save();

                var fileResponse = _mapper.Map<FileResponse>(newFile);
                fileResponses.Add(fileResponse);
            }

            conversation.LastMessege = newMessage.Content;
            _unitOfWork.Save();

            var conversationMessageResponse = _mapper.Map<ConversationMessageResponse>(newMessage);
            conversationMessageResponse.Files = fileResponses;

            return conversationMessageResponse;
        }


        private void PopulateAdditionalFields(ConversationResponse conversationResponse, Conversation conversation)
        {
            // Lấy thông tin bổ sung từ PetOwner
            var petOwner = _unitOfWork.PetOwnerRepository.Get(po => po.AccountId == conversation.PoAccountId,
                                                includeProperties: "Account").FirstOrDefault();
            if (petOwner != null)
            {
                conversationResponse.PoName = petOwner.FullName;
                conversationResponse.PoAvatar = petOwner.Account.Avatar;
            }

            // Lấy thông tin bổ sung từ Store
            var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == conversation.StaffAccountId,
                                                includeProperties: "Brand").FirstOrDefault();
            if (store != null)
            {
                conversationResponse.StoreName = store.Name;
                conversationResponse.StoreAvatar = store.Brand.Logo;
            }
        }
    }
}
