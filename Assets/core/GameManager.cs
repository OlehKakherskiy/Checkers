using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private static readonly int PIECES_IN_ROW = 4;

	public BoardView BoardView;

	private BoardManager boardManager;
	private BoardGenerator boardGenerator;

	private Position from;
	private Color whoGoes;

	void Awake() {
		boardGenerator = new BoardGenerator ();
	}

	// Use this for initialization
	void Start () {
		boardManager = new BoardManager (BoardView, boardGenerator);
		from = null;
		whoGoes = Color.WHITE;
	}

	public void HandleCellClick(GameObject clickedCell) {
		Debug.Log ("WasCelected cell number "+ clickedCell.name);
		Position currentPos = convertNameToCellPosition (clickedCell.name);
		if (secondCellChosen()) {
			Debug.Log ("Position to move cell is " + currentPos);
			if (isMovementSuccessful(currentPos)) {
				if (boardManager.WasRemovedPiece && hasPieceToAttack (currentPos)) {
					from = currentPos;
				} else {
					from = null;
					boardManager.WasRemovedPiece = false;
					switchUser ();
				}
			}
		} else {
			Piece chosenPiece = boardManager.GetPiece(currentPos);
			if (emptyCellWasChosen (chosenPiece)) {
				System.Console.WriteLine("chosen cell is absent");
				return;
			}
			if (isObligedToAttackFromAnotherPosition(currentPos)) {
				return;
			}
			Debug.Log("Chosen piece has a color of " + chosenPiece.Color);
			from = (chosenPiece.Color == whoGoes) ? currentPos : null;
		}
	}

	private Position convertNameToCellPosition(string cellName) {
		if (cellName.StartsWith ("Cell")) {
			int cellNum = System.Convert.ToInt32 (cellName.Split (' ') [1]);
			int y = cellNum / PIECES_IN_ROW;
			int offset = (y % 2 == 1) ? 1 : 0;
			int x = offset + (cellNum % PIECES_IN_ROW) * 2;
			Debug.Log ("Converted position of cell "+ cellNum + "is "+new Position(x,y));
			return new Position (x, y);
		} else {
			return null;
		}
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
