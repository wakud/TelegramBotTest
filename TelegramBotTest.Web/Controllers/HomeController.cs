using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Polling;
using TelegramBotTest.Web.BLL;
using TelegramBotTest.Web.Models;

namespace TelegramBotTest.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IConfiguration _configuration;

        private readonly IBot _bot;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IBot bot)
        {
            _logger = logger;
            _configuration = configuration;
            _bot = bot;
        }

        private void OpenLinkInBrowser(string url)
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        public async void Index()
        {
            OpenLinkInBrowser("https://t.me/free_user_test_bot");

            var botClient = new TelegramBotClient(_configuration["Telegram:BotToken"]);
            using var cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = { }
            }; 

            botClient.StartReceiving(
                _bot.HandleUpdateAsync,
                _bot.HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();
            Console.WriteLine($"Start work with @" + me.Username);
            await Task.Delay(int.MaxValue);
            cts.Cancel();
        } 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}