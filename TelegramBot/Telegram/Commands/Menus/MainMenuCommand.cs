using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.CommandsManager.Commands;

public class MainMenuCommand : BaseCommand
{
	private readonly TelegramBotClient _client;

	public MainMenuCommand(TelegramBotClient client)
	{
		_client = client;
	}
	public override async Task ExecuteAsync(Update update)
	{
		ChatId chat = null;
		
		var inlineMarkup = new InlineKeyboardMarkup(new []
		{
			new []{new InlineKeyboardButton("Сеансы"){CallbackData =((int) CommandNames.SessionMenuCommand).ToString()}},
			new []{new InlineKeyboardButton("Показатели детекторов среды"){CallbackData =((int) CommandNames.EnvironmentMenuCommand).ToString()}},
			new []{new InlineKeyboardButton("Отчетность"){CallbackData =((int) CommandNames.ProtocolsMenuCommand).ToString()}},
		});
		if (update.Type == UpdateType.CallbackQuery)
		{
			chat = update.CallbackQuery.Message.Chat;
			await _client.EditMessageTextAsync(chat,update.CallbackQuery.Message.MessageId, "Здравствуйте! Выберите пункт:");
			await _client.EditMessageReplyMarkupAsync(chat,update.CallbackQuery.Message.MessageId,replyMarkup: inlineMarkup);
		}
		if (update.Type == UpdateType.Message)
		{
			chat = update.Message.Chat;
			await _client.SendTextMessageAsync(chat, "Здравствуйте! Выберите пункт:", replyMarkup: inlineMarkup);

		}
	}
}