using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotTest.Web.BLL
{
    public interface IBot
    {
        Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken);
    }
}
