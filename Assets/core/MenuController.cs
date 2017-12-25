using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

	public GameObject BoardPanel;
	public GameObject MenuPanel;
	private GameManager gameManager;
	public GameObject GameOverPanel;
	public GameObject AchievementsPanel;

	void Start () {
		gameManager = BoardPanel.GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			if (GameOverPanel.activeInHierarchy || AchievementsPanel.activeInHierarchy) 
			{
				//ignore escape on game over panel
				return;
			}
			if (MenuPanel.activeInHierarchy) 
			{
				//continue game
				MenuPanel.SetActive (false);
				Debug.Log ("continue game after pause");
			} else {
				//pause game
				MenuPanel.SetActive (true);
				Debug.Log ("pause game");
			}
		}
	}

	public void ContinueGame() { 
		Debug.Log ("Continue game was pressed");
		MenuPanel.SetActive (false);
	}

	public void StartNewGame() {
		Debug.Log ("Start New Game");
		gameManager.RemoveSavedGame ();
		GameOverPanel.SetActive (false);
		MenuPanel.SetActive (false);
		gameManager.Start ();
	}

	public void SaveAndExit() {
		MenuPanel.SetActive (false);
		GameOverPanel.SetActive (false);
		Debug.Log ("Save And Exit");
		gameManager.SaveGame ();
		Application.Quit ();
	}

	public void Exit() {
		gameManager.RemoveSavedGame ();
		Application.Quit ();
	}

	public void ShowAchievements() {
		MenuPanel.SetActive (false);
		AchievementsPanel.SetActive (true);
	}

	public void ReturnToMenuFromAchievements() {
		MenuPanel.SetActive (true);
		AchievementsPanel.SetActive (false);
	}
}
