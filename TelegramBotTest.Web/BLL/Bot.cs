using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotTest.Web.Repository;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotTest.Web.Data;

namespace TelegramBotTest.Web.BLL
{
    public class Bot : IBot
    {
        private readonly IRepository _repository;

        public Bot(IRepository repository, TelegramDbContext context)
        {
            _repository = repository;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            bool sentToMeMode = false;

            if (update.Type == UpdateType.Message && update.Message!.Type == MessageType.Text)
            {
                var chatId = update.Message.Chat.Id;
                var messageText = update.Message.Text;

                if (messageText == "/start")
                {
                    var sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: $"Ласкаво просимо! Ви можете перевірити вашу підписку на канал за допомогою кнопки.",
                        replyMarkup: GetKeyboard(),
                        cancellationToken: cancellationToken
                    );
                    _repository.RegisterUser(update);
                }
            }
            else if (update.CallbackQuery is { Data: "/get_user" })
            {
                var userStr = string.Empty;
                var user = _repository.GetUser(update);

                if (user != null)
                {
                    string name = string.Empty;
                    DateTime register = DateTime.UtcNow;

                    foreach (var item in user)
                    {
                        name = item.UserName;
                        register = item.CreatedUser;
                    }

                    var sentMessage = await botClient.SendTextMessageAsync(
                        chatId: update.CallbackQuery.Message.Chat.Id,
                        $"Користувач {name} зареєстований на каналі {register.ToString("dd MMMM yyyy")} року.",
                        cancellationToken: cancellationToken
                    );
                }
            }
        }

        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException =>
                    $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private static InlineKeyboardMarkup GetKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Перевірити підписку", "/get_user"),
                },
            });
        }
    }
}
