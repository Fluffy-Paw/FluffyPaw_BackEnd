using FluffyPaw_Application.DTO.Request.ConversationRequest;
using FluffyPaw_Application.DTO.Response.ConversationResponse;
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
        Task<List<ConversationMessageResponse>> GetAllConversationMessageByConversationId(long id);
        Task<ConversationMessageResponse> SendMessage(ConversationMessageRequest conversationMessageRequest);
    }
}
