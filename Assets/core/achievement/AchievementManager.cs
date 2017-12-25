using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour {

	private List<IAchievement> achievements;
	private Dictionary<string, Text> achievementValues;
	public GameObject AchievementPanel;

	public void LoadGame(GameData gameData) {
		achievements = gameData.Achievements;
		if (!object.ReferenceEquals (null, achievementValues) && achievementValues.Count != 0) {
			return;
		}
		achievementValues = new Dictionary<string, Text> ();
		foreach (IAchievement achievement in achievements) {
			foreach (string key in achievement.GetAllAchievements().Keys) {
				Debug.Log ("Achievement txt field name is "+(key.Replace(" ","") + "TxtValue"));
				Debug.Log (GameObject.Find (key.Replace (" ", "") + "TxtValue").GetComponent<Text> ());
				achievementValues.Add (key, GameObject.Find (key.Replace(" ","") + "TxtValue").GetComponent<Text>());
			}
		}
		UpdateAchievements(gameData);
		AchievementPanel.SetActive (false);
	}

	public void UpdateAchievements(GameData gameData) {
		foreach(IAchievement achievement in achievements) {
			achievement.RecalculateAchievement (gameData);
			foreach (string achievementKey in achievement.GetAllAchievements().Keys) {
				achievementValues [achievementKey].text = achievement.GetAllAchievements() [achievementKey] == true ? "done" : "not yet";
			}
		}
	}
}
