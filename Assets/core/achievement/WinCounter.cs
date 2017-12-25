using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WinCounter : IAchievement {
	
	private List<string> achievementName;
	private List<bool> achievementValue;
	private int winCount;

	public WinCounter ()
	{
		achievementName = new List<string> (){"1 game", "5 games", "10 games", "25 games", "50 games", "100 games"};
		achievementValue = new List<bool> (){false, false, false, false, false, false};
		winCount = 0;
	}
	
	public string GetName () {
		return "Winner";
	}

	public string GetDescription() {
		return "Games count won using White pieces";
	}

	public Dictionary<string, bool> GetAllAchievements() {
		Dictionary<string, bool> dictionary = new Dictionary<string, bool> ();
		for (int i = 0; i < achievementName.Count; i++) {
			dictionary.Add (achievementName [i], achievementValue [i]);
		}
		return dictionary;
	}

	public void RecalculateAchievement (GameData gameData) {
		if (gameData.WhoGoes == Color.WHITE && noEnemyPieces(gameData.GameBoard)) {
			winCount++;
			markAchievementSucceded ();
		}
	}

	private void markAchievementSucceded() {
		for (int i = 0; i < achievementName.Count; i++) {
			if (winCount == System.Convert.ToInt32 (achievementName[i].Split (' ') [0])) {
				achievementValue [i] = true;
			}
		}
	}

	private bool noEnemyPieces(Piece[,] board) {
		for (int y = 0; y < 8; y++) {
			for (int x = 0; x < 8; x++) {
				if (board [x, y] != null && board [x, y].Color == Color.BLACK) {
					return false;
				}
			}
		}
		return true;
	}

}
