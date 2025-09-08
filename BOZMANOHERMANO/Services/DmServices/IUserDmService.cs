using BOZMANOHERMANO.Dtos;
using BOZMANOHERMANO.Models;
using BOZMANOHERMANO.Repo;
using StartUp.HiddenServices;

namespace BOZMANOHERMANO.Services.DmServices
{
    public interface IUserDmService
    {
        IEnumerable<DmDto> GetUserMessages();
        ChatDto OpenChat(string senderId);

        string SendMessage(string message, string recId);
    }
    public class UserDmService : IUserDmService
    {
        private readonly IUserDmRepo _repo;
        private readonly IUserContext _userContext;

        public UserDmService(IUserDmRepo repo,
            IUserContext userContext)
        {
            _repo = repo;
            _userContext = userContext;
        }

        public IEnumerable<DmDto> GetUserMessages()
        {
            return _repo.GetMessages(_userContext.GetUserId())
                .Select(a => new DmDto()
                {
                    Id = a.Id,
                    Message = a.Message,
                    SenderId = a.SenderId,
                    UserName = a.Sender.UserName,
                    TagName = a.Sender.TagName,
                    MessageDate = a.MessageDate
                });
        }

        public ChatDto OpenChat(string senderId)
        {
            var recId = _userContext.GetUserId();

            var messages = _repo.OpenChat(senderId, recId);

            var chat = new ChatDto
            {
                Id = messages.First().Id,
                SenderId = senderId,
                UserName = messages.First().Sender.UserName,
                TagName = messages.First().Sender.TagName,
                Messages = messages
            .Select(m => new MessagesDto
            {
                Message = m.Message,
                MessageDate = m.MessageDate
            })
            .OrderBy(m => m.MessageDate)
            .ToList()
            };

            return chat;
        }

        public string SendMessage(string message, string recId)
        {
            var messages = new UserDM()
            {
                Message = message,
                SenderId = _userContext.GetUserId(),
                RecieverId = recId,
                MessageDate = DateTime.UtcNow

            };
            return _repo.SendMessage(messages);
        }
    }
}
