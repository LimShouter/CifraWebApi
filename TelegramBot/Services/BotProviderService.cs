using Telegram.Bot;

namespace TelegramBot.Services;

public class BotProviderService
{
	public static BotProviderService Main;
	public static TelegramBotClient Bot;

	public BotProviderService(IConfiguration configuration)
	{
		Bot = new TelegramBotClient(configuration["Token"]);
		Bot.SetWebhookAsync(configuration["Url"] + "api/message/update");
	}
}