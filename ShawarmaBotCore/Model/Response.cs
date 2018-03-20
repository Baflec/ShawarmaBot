using Telegram.Bot.Types.ReplyMarkups;

namespace ShawarmaBotCore.Model
{
    public class Response
    {
        public long ChatId { get; set; }
        public string Text { get; set; }
        public IReplyMarkup ReplyMarkup { get; set; }
    }
}
