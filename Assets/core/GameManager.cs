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
		if (secondCellChosen()) { //identifying position to move <chosenPiece>
			Debug.Log ("Position to move cell is " + currentPos);
			if (boardManager.GetPiece (from).doMovement (from, currentPos) == true) {
				from = null;
				switchUser ();
			}
		} else { //identifying whether a cell with appropriate Color
			Piece pieceToChoose = boardManager.GetPiece(currentPos);
			if (emptyCellWasChosen (pieceToChoose)) {
				System.Console.WriteLine("chosen cell is absent");
				return;
			}
			Debug.Log("Chosen piece has a color of " + pieceToChoose.Color);
			from = (pieceToChoose.Color == whoGoes) ? currentPos : null;
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

	private void switchUser() {
		whoGoes = (Color.WHITE == whoGoes) ? Color.BLACK : Color.WHITE;
	}
}
