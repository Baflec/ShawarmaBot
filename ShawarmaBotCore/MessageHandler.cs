using System.Collections.Generic;
using System.Linq;
using ShawarmaBotCore.Model;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;
using User = ShawarmaBotCore.Model.User;

namespace ShawarmaBotCore
{
    public class MessageHandler
    {
        public MessageHandler()
        {
        }

        public Response Handle(Update update)
        {
            var user = GetUser(update) ?? CreateUser(update);

            var text = update.Message.Text;

            switch (text)
            {
                case null:
                    return null;
                case "/start":
                {
                    var buttons = new List<string>
                    {
                        "Сделать заказ"
                    };

                    if (user.Order.OrderItems.Count != 0)
                    {
                        buttons.Add("Корзина");
                    }

                    return new Response
                    {
                        ChatId = update.Message.Chat.Id,
                        Text = "Добро пожаловать, я чат-бот шаурмешной Универ!\n" +
                               "\n" +
                               "Хочешь сделать заказ?",
                        ReplyMarkup = CreateReplyKeyboardMarkup(buttons)
                    };
                }
                case "Сделать заказ":
                {
                    var topCategories = GetTopCategories();

                    var buttons = topCategories.Select(category => category.Name).ToList();

                    if (user.Order.OrderItems.Count != 0)
                    {
                        buttons.Add("Корзина");
                    }

                    buttons.Add("Назад");

                    return new Response
                    {
                        ChatId = update.Message.Chat.Id,
                        Text = "Выберите категорию еды, которую вы хотите заказать",
                        ReplyMarkup = CreateReplyKeyboardMarkup(buttons)
                    };
                }
            }

            foreach (var topCategory in GetTopCategories())
            {
                if (text.Equals(topCategory.Name))
                {
                    using (var db = new ApplicationContext())
                    {
                        var childCategories =
                            db.Categories.Where(category => category.Parent.Id == topCategory.Id);

                        if (childCategories.Any())
                        {
                            return new Response
                            {
                                ChatId = update.Message.Chat.Id,
                                Text = "Выберете подкатегорию",
                                ReplyMarkup =
                                    CreateReplyKeyboardMarkup(
                                        childCategories.Select(category => category.Name).ToList())
                            };
                        }

                        var itemsByCategory = db.Items.Where(item => item.Category.Id == topCategory.Id)
                            .Select(item => item.Name).ToList();

                        return new Response
                        {
                            ChatId = update.Message.Chat.Id,
                            Text = "Выберете товар",
                            ReplyMarkup =
                                CreateReplyKeyboardMarkup(itemsByCategory)
                        };
                    }
                }
            }

            using (var db = new ApplicationContext())
            {
                var item = db.Items.FirstOrDefault(i => i.Name == text);

                if (item != null)
                {
                    return new Response
                    {
                        ChatId = update.Message.Chat.Id,
                        Text = $"{item.Name}\n" +
                               "\n" +
                               $"{item.Description}\n" +
                               "\n" +
                               $"{item.Price}",
                        ReplyMarkup = new InlineKeyboardMarkup(
                            new InlineKeyboardButton[]
                            {
                                "Добавить"
                            })
                    };
                }
            }

            return new Response();
        }

        private List<Category> GetTopCategories()
        {
            using (var db = new ApplicationContext())
            {
                return db.Categories.Where(category => category.Parent == null)
                    .ToList();
            }
        }

        private User CreateUser(Update update)
        {
            using (var db = new ApplicationContext())
            {
                var user = new User
                {
                    ChatId = update.Message.Chat.Id
                };

                db.Users.Add(user);
                db.SaveChanges();

                return user;
            }
        }

        public User GetUser(Update update)
        {
            using (var db = new ApplicationContext())
            {
                return db.Users.FirstOrDefault(user => user.ChatId == update.Message.Chat.Id);
            }
        }

        public ReplyKeyboardMarkup CreateReplyKeyboardMarkup(List<string> buttons)
        {
            var keyboardButtons = buttons.Select(button => new KeyboardButton(button)).ToArray();

            var keyboardArray = new KeyboardButton[keyboardButtons.Count()][];

            for (var i = 0; i < keyboardArray.Length; i++)
            {
                keyboardArray[i] = new[] {keyboardButtons[i]};
            }

            return new ReplyKeyboardMarkup(keyboardArray, resizeKeyboard: true, oneTimeKeyboard: true);
        }
    }
}