using Telegram.Bot.Types;

namespace TelegramBotTest.Web.Repository
{
    public interface IRepository
    {
        public List<(string UserName, DateTime CreatedUser)> GetUser(Update update);
        public void RegisterUser(Update update);
    }
}
