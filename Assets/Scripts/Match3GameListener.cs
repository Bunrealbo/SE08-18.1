using System;

public interface Match3GameListener
{
	void OnGameComplete(GameCompleteParams completeParams);

	void OnGameStarted(GameStartedParams startedParams);
}
