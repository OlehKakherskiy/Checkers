using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private static readonly int PIECES_IN_ROW = 4;

	public BoardView BoardView;
	public GameObject GameOverPanel;
	public AchievementManager AchievementManager;
	public Text WhoWonText;

	private BoardManager boardManager;
	private BoardGenerator boardGenerator;

	private Position from;
	private Color whoGoes;

	private GameProceedingStrategy gameProceedingStrategy;
	private GameData gameData;

	void Awake() {
		boardGenerator = new BoardGenerator ();
		gameProceedingStrategy = new FileGameProceedStrategy (boardGenerator);
	}

	// Use this for initialization
	public void Start () {
		boardManager = new BoardManager (BoardView);
		gameData = gameProceedingStrategy.Load (boardManager);
		boardManager.LoadGame (gameData);
		AchievementManager.LoadGame (gameData);
		from = gameData.SelectedPiece;
		whoGoes = gameData.WhoGoes;
	}

	public void HandleCellClick(GameObject clickedCell) {
		Debug.Log ("WasCelected cell number "+ clickedCell.name);
		Position currentPos = convertNameToCellPosition (clickedCell.name);
		if (secondCellChosen()) {
			if (isMovementSuccessful(currentPos)) {
				if (boardManager.WasRemovedPiece && hasPieceToAttack (currentPos)) {
					from = currentPos;
				} else {
					checkGameOver ();
					from = null;
					boardManager.WasRemovedPiece = false;
					switchUser ();
				}
			}
		} else {
			Piece chosenPiece = boardManager.GetPiece(currentPos);
			if (emptyCellWasChosen (chosenPiece)) {
				return;
			}
			if (chosenPiece.Color != whoGoes || isObligedToAttackFromAnotherPosition(currentPos)) {
				return;
			}
			from = currentPos;
		}
	}

	public void SaveGame() {
		GameData gameData = new GameData ();
		gameData.SelectedPiece = this.from;
		gameData.WhoGoes = this.whoGoes;
		gameData.Achievements = this.gameData.Achievements;

		boardManager.SaveData (gameData);

		gameProceedingStrategy.Save (gameData);
	}

	public void RemoveSavedGame() {
		gameProceedingStrategy.RemoveSavedGame (gameData);
	}

	private Position convertNameToCellPosition(string cellName) {
		if (cellName.StartsWith ("Cell")) {
			int cellNum = System.Convert.ToInt32 (cellName.Split (' ') [1]);
			int y = cellNum / PIECES_IN_ROW;
			int offset = (y % 2 == 1) ? 1 : 0;
			int x = offset + (cellNum % PIECES_IN_ROW) * 2;
			return new Position (x, y);
		} else {
			return null;
		}
	}

	private void checkGameOver() {
		Color enemyColor = (whoGoes == Color.WHITE) ? Color.BLACK : Color.WHITE;
		if (boardManager.AllPiecesAreEaten (enemyColor)) {
			GameOverPanel.SetActive (true);
			WhoWonText.text = whoGoes + " PLAYER WON THE GAME";
			updateAchievements ();
		}
	}

	private void updateAchievements() {
		gameData.SelectedPiece = this.from;
		gameData.WhoGoes = this.whoGoes;
		boardManager.SaveData (gameData);
		AchievementManager.UpdateAchievements (gameData);
	}

	private bool secondCellChosen() {
		return from != null;
	}

	private bool emptyCellWasChosen(Piece piece) {
		return piece == null;
	}

	private bool isMovementSuccessful(Position to) {
		return boardManager.GetPiece (from).doMovement (from, to) == true;
	}

	private bool hasPieceToAttack(Position position) {
		return boardManager.HasPieceToAttack (position);
	}

	private bool isObligedToAttackFromAnotherPosition(Position position) {
		return boardManager.ShouldAttackFromAnotherPosition (position);
	}

	private void switchUser() {
		whoGoes = (Color.WHITE == whoGoes) ? Color.BLACK : Color.WHITE;
	}
}
