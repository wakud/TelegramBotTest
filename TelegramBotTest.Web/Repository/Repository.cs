using Telegram.Bot.Types;
using TelegramBotTest.Web.Data;

namespace TelegramBotTest.Web.Repository
{
    public class Repository : IRepository
    {
        private readonly TelegramDbContext _context;

        /// <summary>
        /// Get user by ChatId
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public List<(string UserName, DateTime CreatedUser)> GetUser(Update update)
        {
            var user = _context.Users.FirstOrDefault(x => x.ChatId == update.CallbackQuery.Message.Chat.Id);
            var userList = new List<(string, DateTime)>();

            if (user != null)
            {
                userList.Add((user.UserName, user.CreatedUser));
            }

            return userList;
        }

        /// <summary>
        /// User registration in the database 
        /// </summary>
        /// <param name="update"></param>
        public void RegisterUser(Update update)
        {
            var userExist = IsUserExist(update.Message.Chat.Id);

            if (userExist)
            {
                var user = _context.Users.FirstOrDefault(x => x.ChatId == update.Message.Chat.Id);
                if (user != null)
                {
                    user.CreatedUser = DateTime.Now;
                    _context.SaveChanges();
                }
            }
            else
            {
                var newUser = new Models.User 
                {
                    ChatId = update.Message.Chat.Id,
                    UserName = update.Message.From.Username,
                    FirstName = update.Message.From.FirstName,
                    LastName = update.Message.From.LastName,
                    CreatedUser = DateTime.Now
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Checking if the user is in the database
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        private bool IsUserExist(long chatId)
        {
            var user = _context.Users.FirstOrDefault(x => x.ChatId == chatId);
            return user != null;
        }
    }
}
