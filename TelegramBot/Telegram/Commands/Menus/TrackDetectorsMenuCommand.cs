using Newtonsoft.Json;
using SharedLibrary;
using Telegram.Bot.Types;

namespace TelegramBot.CommandsManager.Commands;

public class TrackDetectorsMenuCommand : BaseCommand
{
	public override async Task ExecuteAsync(Update update)
	{
		var rawCallbackData = JsonConvert.DeserializeObject<Dictionary<string,string>>(update.CallbackQuery.Data);
		var sessionData = JsonConvert.DeserializeObject<SessionData>(rawCallbackData["sessionData"]);
	}
}