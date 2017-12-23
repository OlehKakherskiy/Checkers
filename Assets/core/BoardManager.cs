using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager {

	private Piece[,] boardModel;
	private BoardView boardView;

	public BoardManager(BoardView boardView, BoardGenerator boardGenerator) {
		this.boardView = boardView;
		boardModel = boardGenerator.GenerateStartGameBoard (this);
		boardView.InitView (boardModel);
	}

	public void MoveChecker(Piece piece, Position From, Position To) {
		boardModel [To.X, To.Y] = piece;
		boardView.UpdateCellView (piece, To);

		RemoveChecker(From);
	}

	public void RemoveChecker(Position position) {
		boardModel [position.X, position.Y] = null;
		boardView.RemovePiece (position);
	}

	public Piece GetPiece(Position position) {
		return boardModel [position.X, position.Y];
	}

	public List<Position> GetCheckersBetween(Position From, Position To) {
		List<Position> notFreePositions = new List<Position> ();

		int startY = -1;
		int endY = -1;
		if (From.Y < To.Y) {
			startY = From.Y;
			endY = To.Y;
		} else {
			startY = To.Y;
			endY = From.Y;
		}

		int startX = -1;
		int endX = -1;
		if (From.X < To.X) {
			startX = From.X;
			endX = To.X;
		} else {
			startX = To.X;
			endX = From.X;
		}

		for (int dY = startY + 1; dY < endY; dY += 1) {
			for (int dX = startX + 1; dX < endX; dX += 1) {
				Debug.Log ("Checking cell number " + new Position(dX, dY));
				if (boardModel [dX, dY] != null) {
					notFreePositions.Add (new Position(dX, dY));
				}
			}
		}
		return notFreePositions;
	}
}
