using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.CommandsManager.Commands;

public class ProtocolsMenuCommand : BaseCommand
{
	private readonly TelegramBotClient _client;

	public ProtocolsMenuCommand(TelegramBotClient client)
	{
		_client = client;
	}

	public override async Task ExecuteAsync(Update update)
	{
		
		var inlineMarkup = new InlineKeyboardMarkup(new[]
		{
			new []{new InlineKeyboardButton("назад"){CallbackData = ((int)CommandNames.MainMenuCommand).ToString()}},
		});
		await _client.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id,update.CallbackQuery.Message.MessageId,"Введите email и номер сеанса"); 
		await _client.EditMessageReplyMarkupAsync(update.CallbackQuery.Message.Chat.Id,update.CallbackQuery.Message.MessageId,inlineMarkup);
	}
}