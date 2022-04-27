using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.CommandsManager.Commands;

public class ConfirmedCommand : BaseCommand
{
	private readonly TelegramBotClient _client;

	public ConfirmedCommand(TelegramBotClient client)
	{
		_client = client;
	}
	public override async Task ExecuteAsync(Update update)
	{
		if (update.Message?.Chat.PinnedMessage == null)
		{
			return;
		}

		var markup = new InlineKeyboardMarkup(new []
		{
			new []{new InlineKeyboardButton( "В меню"){CallbackData = CommandNames.MainMenuCommand.ToString()}}
		});
		await _client.EditMessageTextAsync(update.Message.Chat.Id, update.Message.Chat.PinnedMessage.MessageId, "Готово...");
		await _client.EditMessageReplyMarkupAsync(update.Message.Chat.Id, update.Message.Chat.PinnedMessage.MessageId, markup);
	}
}