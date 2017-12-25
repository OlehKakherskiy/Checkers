using System.Collections;
using System.Collections.Generic;

public interface IAchievement {

	string GetName ();
	string GetDescription();
	Dictionary<string, bool> GetAllAchievements();
	void RecalculateAchievement (GameData gameData);
}
