using Telegram.Bot.Types;

namespace TelegramBot.Services;

public class ObserverCommandService
{
	public Dictionary<ChatId,BaseObserver> Observers = new Dictionary<ChatId, BaseObserver>();

	public ObserverCommandService()
	{
		
	}

	public void Attach(ChatId chatId, BaseObserver observer)
	{
		Observers.Add(chatId,observer);
		observer.Confirmed += Detach;
	}

	public void Check(Update update)
	{
		if (Observers.ContainsKey(update.Message.Chat))
		{
			return;
		}
		Observers[update.Message.Chat].Check(update);
	}

	void Detach(ChatId id)
	{
		Observers[id].Confirmed -= Detach;
		Observers.Remove(id);
	}
}