using System.Collections;
using System.Collections.Generic;

public interface GameProceedingStrategy {

	void Save(GameData gameData);

	GameData Load(BoardManager boardManager);

	void RemoveSavedGame(GameData gameData);
}
