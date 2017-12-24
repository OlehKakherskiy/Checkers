using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager {

	private Piece[,] boardModel;
	private BoardView boardView;

	private List<Position> whitePiecePositions;
	private List<Position> blackPiecePositions;
	private Dictionary<Color, List<Position>> piecePositions;

	private bool wasRemovedPiece;

	public bool WasRemovedPiece {
		get {
			return this.wasRemovedPiece;
		}
		set {
			wasRemovedPiece = value;
		}
	}

	public BoardManager(BoardView boardView, BoardGenerator boardGenerator) {
		this.boardView = boardView;
		boardModel = boardGenerator.GenerateStartGameBoard (this);

		piecePositions = new Dictionary<Color, List<Position>> ();
		piecePositions.Add (Color.WHITE, new List<Position> ());
		piecePositions.Add (Color.BLACK, new List<Position> ());

		boardView.InitView (boardModel);
		extractPieces ();
		WasRemovedPiece = false;
	}

	private void extractPieces() {
		for (int y = 0; y < 8; y++) {
			int startPos = (y % 2 == 0) ? 0 : 1;
			for (int x = startPos; x < 8; x += 2) {
				Position position = new Position (x, y);
				Piece p = GetPiece (position);
				if (!object.ReferenceEquals(null, p)) {
					piecePositions [p.Color].Add (position);
				}
			}
		}
	}

	public void MoveChecker(Piece piece, Position From, Position To) {
		boardModel [To.X, To.Y] = piece;
		piecePositions [piece.Color].Add (To);

		boardView.UpdateCellView (piece, To);

		RemoveChecker(From);
	}

	public void RemoveChecker(Position position) {
		piecePositions [GetPiece (position).Color].Remove (position);
		boardModel [position.X, position.Y] = null;
		boardView.RemovePiece (position);
	}

	public Piece GetPiece(Position position) {
		return boardModel [position.X, position.Y];
	}

	public List<Position> GetCheckersBetween(Position From, Position To) {
		List<Position> positions = new List<Position> ();

		if (To.X >= 8 || To.X < 0 || To.Y >= 8 || To.Y < 0 || From.Equals(To)) {
			return positions;
		}

		int deltaX = (From.X < To.X) ? +1 : -1;
		int deltaY = (From.Y < To.Y) ? +1 : -1;

		int dX = From.X + deltaX;
		for (int dY = From.Y + deltaY; (deltaY > 0 && dY <= To.Y) || (deltaY < 0 && dY >= To.Y); dY += deltaY) { 
			if (object.ReferenceEquals(null, boardModel [dX, dY])) {
				positions.Add (null);
			} else {
				positions.Add (new Position (dX, dY));
			}
			dX += deltaX;
		}
		return positions;
	}

	public bool HasPieceToAttack(Position position) {
		Piece piece = GetPiece (position);
		List<List<Position>> possibleAttackVectors = getPossibleAttackPositions (position, piece);
		foreach (List<Position> diagonalSlice in possibleAttackVectors) {
			if (diagonalSlice.Count >= 2 
				&& !object.ReferenceEquals(null, diagonalSlice[0]) 
				&& object.ReferenceEquals(null, diagonalSlice[1]) 
				&& GetPiece (diagonalSlice [0]).Color != piece.Color) {
				return true;
			}
		}
		return false;
	}

	public bool ShouldAttackFromAnotherPosition(Position position) {
		if (HasPieceToAttack (position)) {
			return false;
		}
		foreach (Position anotherPosition in piecePositions[GetPiece (position).Color]) {
			if (!position.Equals (anotherPosition) && HasPieceToAttack(anotherPosition)) {
				return true;
			}
		}
		return false;
	}

	public bool AllPiecesAreEaten(Color color){
		return piecePositions [color].Count == 0;
	}

	private List<List<Position>> getPossibleAttackPositions(Position position, Piece piece) {
		List<List<Position>> possibleAttackPositions = new List<List<Position>> ();
		Position[] diagonalPositions = new Position[4];
		diagonalPositions [0] = getDiagonalPosition (position, 1, 1);    // up right diagonal
		diagonalPositions [1] = getDiagonalPosition (position, -1, 1);   // up left diagonal
		diagonalPositions [2] = getDiagonalPosition (position, 1, -1);   // down right diagonal
		diagonalPositions [3] = getDiagonalPosition (position, -1, -1);  // down left diagonal 
		for (int i = 0; i < diagonalPositions.Length; i++) {
			possibleAttackPositions.Add (GetCheckersBetween (position, diagonalPositions [i]));
		}
		return possibleAttackPositions;
	}

	private Position getDiagonalPosition(Position pos, int deltaX, int deltaY) { //off iterations count => maxIterations = -1
		int maxIterations = (GetPiece(pos).CheckerType == Type.PAWN) ? 2 : -1;
		Position diagonalPosition = new Position(pos.X, pos.Y);
		int endX = (deltaX > 0) ? 7 : 0;
		int endY = (deltaY > 0) ? 7 : 0;
	
		while(diagonalPosition.X != endX && diagonalPosition.Y != endY) {
			if (maxIterations == 0) {
				break;
			}
			maxIterations--;
			diagonalPosition.X += deltaX;
			diagonalPosition.Y += deltaY;
		}
		return diagonalPosition;
	}
}
