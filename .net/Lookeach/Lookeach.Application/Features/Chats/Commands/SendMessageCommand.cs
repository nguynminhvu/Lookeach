using Lookeach.Application.AutoMappers;
using Lookeach.Application.Features.Chats.Models;
using Lookeach.Application.Interfaces;
using Lookeach.Application.Interfaces.Services;
using Lookeach.Application.Services;
using Lookeach.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lookeach.Application.Features.Chats.Commands
{
    public record SendMessageCommand(ChatMessageSendModel ChatMessageSendModel) : IRequest<ChatViewModel>
    {
    }
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, ChatViewModel>
    {
        private readonly IFirebaseService _firebaseService;
        private readonly IValidateService _validateService;
        private readonly IIdentityUser<Guid> _identityUser;
        private readonly ICleanContext _context;
        private const string CHAT_NAME_DEFAULT = "Default Name";
        private string[] _permittedExtensions = { ".png", ".jpg", ".jpeg" };
        private const string CHAT_IMAGE = "ChatImage";
        public SendMessageCommandHandler(ICleanContext cleanContext, IIdentityUser<Guid> identityUser, IValidateService validateService, IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
            _validateService = validateService;
            _identityUser = identityUser;
            _context = cleanContext;
        }

        public async Task<ChatViewModel> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            List<string> subUrl = new List<string>();
            try
            {
                if (_identityUser.UserId == Guid.Empty)
                {
                    throw new UnauthorizedAccessException("Login session expired");
                }
                if (request == null || request.ChatMessageSendModel == null!) throw new Exception("Miss information");

                var participantChat = await _context.ParticipantChats.Where(x => x.SenderId == _identityUser.UserId && x.ReceiverId == request.ChatMessageSendModel!.ReceiverId).ToListAsync();
                Guid chatId = Guid.Empty;
                if (participantChat != null && participantChat.Count > 0)
                {
                    chatId = participantChat.FirstOrDefault()!.ChatId;
                }
                if (chatId == Guid.Empty)
                {
                    chatId = Guid.NewGuid();
                    Chat chat = new Chat
                    {
                        Id = chatId,
                        Name = CHAT_NAME_DEFAULT,
                    };
                    await _context.Chats.AddAsync(chat);
                    List<ChatUser> chatUsers = new List<ChatUser>();
                    chatUsers.Add(new ChatUser
                    {
                        UserId = _identityUser.UserId,
                        ChatId = chatId,
                        Id = Guid.NewGuid()
                    });
                    chatUsers.Add(new ChatUser
                    {
                        UserId = request.ChatMessageSendModel.ReceiverId,
                        ChatId = chatId,
                        Id = Guid.NewGuid()
                    });
                    await _context.ChatUsers.AddRangeAsync(chatUsers);
                    ParticipantChat participantChat1 = new ParticipantChat
                    {
                        SenderId = _identityUser.UserId,
                        ReceiverId = request.ChatMessageSendModel.ReceiverId,
                        ChatId = chatId,
                        Id = Guid.NewGuid()
                    };
                    await _context.ParticipantChats.AddAsync(participantChat1);
                }
                if (request.ChatMessageSendModel != null && request.ChatMessageSendModel.ChatImages != null && request.ChatMessageSendModel.ChatImages.Count > 0)
                {
                    await _validateService.ValidImage(request.ChatMessageSendModel.ChatImages.ToList(), _permittedExtensions);
                    foreach (var item in request.ChatMessageSendModel.ChatImages)
                    {
                        var url = await _firebaseService.UploadToFirebase(item, CHAT_IMAGE);
                        if (url == null) throw new Exception("Upload image fail");
                        subUrl.Add(url);
                        Image image = Image.Create(url, _identityUser.UserId, _identityUser.UserId);
                        await _context.Images.AddAsync(image);
                        ChatImage chatImage = new ChatImage
                        {
                            ChatId = chatId,
                            ImageId = image.Id
                        };
                        await _context.ChatImages.AddAsync(chatImage);
                    }
                }
                ChatMessage chatMessage = new ChatMessage
                {
                    Id = Guid.NewGuid(),
                    ChatId = chatId,
                    Text = request.ChatMessageSendModel!.Text,
                    UserId = _identityUser.UserId
                };
                await _context.ChatMessages.AddAsync(chatMessage);
                await _context.SaveChangesAsync(new CancellationToken());
                var rs = await _context.Chats.Include(x => x.Messages).ThenInclude(x => x.User)
                    .AsNoTracking().Include(x => x.Messages).ThenInclude(x => x.ChatImages)
                    .AsNoTracking().Include(x => x.ChatUsers).AsNoTracking().FirstOrDefaultAsync(x => x.Id == chatId);
                if (rs != null && rs.Messages.Count > 0)
                {
                    rs.Messages = rs.Messages.OrderByDescending(x => x.ModifiedOn).ToList();
                }
                else if (rs == null)
                {
                    throw new Exception("Send message fail");
                }
                return AutoMapperConfig.Mapper.Map<ChatViewModel>(rs);
            }
            catch (Exception ex)
            {
                foreach (var item in subUrl)
                {
                    await _firebaseService.DeleteImage(item, CHAT_IMAGE);
                }
                throw new Exception(ex.Message);
            }
        }
    }
}
