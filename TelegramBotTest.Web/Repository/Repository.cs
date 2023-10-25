using Microsoft.Data.SqlClient;
using Telegram.Bot.Types;

namespace TelegramBotTest.Web.Repository
{
    public class Repository : IRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public Repository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<(string UserName, DateTime CreatedUser)> GetUser(Update update)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var users = new List<(string, DateTime)>();

                connection.Open();
                var command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT UserName, CreatedUser FROM AppUser WHERE ChatId LIKE '{update.Message.Chat.Id}'";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string userName = reader.GetString(0);
                    DateTime createdUser = reader.GetDateTime(1);
                    users.Add((userName, createdUser));
                }

                return users;
            }
        }

        public void RegisterUser(Update update)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand();
            command.Connection = connection;
            if (IsUserExist(update.Message.Chat.Id))
            {
                command.CommandText = $"UPDATE AppUser SET CreatedUser = '{DateTime.Now.ToString("yyyy-MM-dd")}' " +
                                      $"WHERE FirstName LIKE '{update.Message.From.FirstName}'";
            }
            else
            {
                command.CommandText = $"INSERT INTO dbo.AppUser(ChatId, UserName, FirstName, LastName, CreatedUser)" +
                                      $"VALUES('{update.Message.Chat.Id}', N'{update.Message.From.Username}', N'{update.Message.From.FirstName}', " +
                                      $"'{update.Message.From.LastName}', '{DateTime.Now.ToString("yyyy-MM-dd")}')";
            }

            command.ExecuteNonQuery();
        }

        private bool IsUserExist(long chatId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT 1 FROM AppUser WHERE ChatId LIKE '{chatId}'";
                return command.ExecuteScalar() != null;
            }
        }
    }
}
