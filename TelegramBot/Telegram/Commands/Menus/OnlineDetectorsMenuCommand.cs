using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.CommandsManager.Commands;

public class OnlineDetectorsMenuCommand : BaseCommand
{

	public override async Task ExecuteAsync(Update update)
	{
		var chat = update.Message.Chat;
		var pinnedMessage = chat.PinnedMessage;
		var sessionNumber = pinnedMessage.Text.Split(" ")[0];

		var markup = new InlineKeyboardMarkup(new []
		{
			new []{new InlineKeyboardButton("Назад"){CallbackData = CommandNames.SessionDetailsMenuCommand + " " + sessionNumber}}
		});
	}
}