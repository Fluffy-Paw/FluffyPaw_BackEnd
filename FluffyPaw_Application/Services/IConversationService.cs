using FluffyPaw_Application.DTO.Request.ConversationRequest;
using FluffyPaw_Application.DTO.Response.ConversationResponse;
using FluffyPaw_Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IConversationService
    {
        Task<List<ConversationResponse>> GetAllConversation();
        Task<ConversationResponse> CreateConversation(ConversationRequest conversationRequest);
        Task<bool> OpenConversation(long id);
        Task<bool> CloseConversation(long id);
        Task<bool> DeleteConversation(long id);
        Task<IPaginatedList<ConversationMessageResponse>> GetAllConversationMessageByConversationId(long id, int pageNumber, int pageSize);
        Task<ConversationMessageResponse> SendMessage(ConversationMessageRequest conversationMessageRequest);
    }
}
