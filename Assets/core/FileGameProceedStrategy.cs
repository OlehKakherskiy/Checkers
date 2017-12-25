using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

public class FileGameProceedStrategy : GameProceedingStrategy {

	private BoardGenerator boardGenerator;

	public FileGameProceedStrategy (BoardGenerator boardGenerator)
	{
		this.boardGenerator = boardGenerator;
	}
	
	public void Save(GameData gameData) {
		string saveToFile = Application.persistentDataPath + "/savedGame.gd";
		Debug.Log("Save game to file = "+saveToFile);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (saveToFile);
		bf.Serialize(file, gameData);
		file.Close();

		Debug.Log ("Game Data was saved");
	}

	public GameData Load(BoardManager boardManager) {
		GameData gameData = null;
		string loadFromFile = Application.persistentDataPath + "/savedGame.gd";
		bool fileExists = File.Exists (loadFromFile);
		if (fileExists) {
			Debug.Log ("File '" + loadFromFile + "' exist, so loading game");
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/savedGame.gd", FileMode.Open);
			gameData = (GameData)bf.Deserialize (file);
			file.Close ();
		} else {
			Debug.Log ("File '" + loadFromFile + "' doesn't exist, so load from stub");
			gameData = new GameData ();
		}
		completeBoardModelPreparation (boardManager, gameData, fileExists);
		return gameData;
	}

	private void completeBoardModelPreparation(BoardManager boardManager, GameData gameData, bool fileExists) {
		if (!fileExists || gameData.GameBoard.Length == 0) {
			gameData.GameBoard = boardGenerator.GenerateStartGameBoard (boardManager);
		} else {
			gameData.GameBoard = boardGenerator.GenerateStartGameBoard (boardManager, gameData.GameBoard);
		}
	}

	public void RemoveSavedGame(GameData gameData)
	{
		gameData.GameBoard = new Piece[0, 0];
		gameData.SelectedPiece = null;
		gameData.WasEatenEnemyPiece = false;
		gameData.WhoGoes = Color.WHITE;

		Save (gameData);
	}
}
