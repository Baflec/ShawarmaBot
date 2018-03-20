using System.Linq;
using Telegram.Bot;

namespace ShawarmaBotCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var telegramBotClient = new TelegramBotClient("588647415:AAE5E__JN27P8_bKLq0EiRujzGEG8F7ZSSw");

            var messageHandler = new MessageHandler();


            var offset = 0;

            while (true)
            {
                var updates = telegramBotClient.GetUpdatesAsync(offset: offset, limit: 100, timeout: 100).Result;

                if (!updates.Any())
                {
                    continue;
                }

                offset = updates.Max(update => update.Id) + 1;

                foreach (var update in updates)
                {
                    var response = messageHandler.Handle(update);

                    if (response == null)
                    {
                        continue;
                    }

                    telegramBotClient.SendTextMessageAsync(response.ChatId, response.Text,
                        replyMarkup: response.ReplyMarkup);
                }
            }
        }
    }
}