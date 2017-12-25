using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum Color
{
	WHITE = 1, BLACK = -1
}
public enum Type {
	PAWN,
	KING
}
[System.Serializable]
public class Piece {

	private static readonly int SIMPLE_MOVEMENT = 1;
	private static readonly int FIGHT_MOVEMENT = 2;
	private static readonly int MAX_VALID_CHECKERS_COUNT_BETWEEN_POSITIONS = 1;

	private Color color;
	private Type checkerType;

	[System.NonSerialized]
	private BoardManager boardManager;

	public Color Color {
		get {
			return this.color;
		}
	}

	public Type CheckerType {
		get {
			return this.checkerType;
		}
	}

	public BoardManager BoardManager {
		get {
			return this.boardManager;
		}
		set {
			boardManager = value;
		}
	}

	public Piece(Color color, Type type, BoardManager boardManager) {
		this.color = color;
		this.checkerType = type;
		this.boardManager = boardManager;
	}

	public bool doMovement(Position From, Position To) {
		List<Position> pieceBetweenPositions = boardManager.GetCheckersBetween (From, To);
		pieceBetweenPositions.RemoveAll (position => object.ReferenceEquals(null, position));
		pieceBetweenPositions.RemoveAll (position => To.Equals (position));

		if (!basicValidationIsOk(From, To)) {
			return false;
		}
		if (isFightMovement (From, To, pieceBetweenPositions)) {
			if (!isEnemyChecker (pieceBetweenPositions [0]))
				return false;
			performFightMovement (From, To, pieceBetweenPositions);
			boardManager.WasRemovedPiece = true;
		} else {
			if (boardManager.HasPieceToAttack (From)) {
				return false;
			}
			performSimpleMovement (From, To);
			boardManager.WasRemovedPiece = false;
		}
		return true;
	}

	private Boolean basicValidationIsOk(Position From, Position To) {
		int DeltaX = Math.Abs (From.X - To.X);
		int DeltaY = Math.Abs (From.Y - To.Y);
		if(!object.ReferenceEquals(null, boardManager.GetPiece(To))) {
			return false;
		}
		if (this.checkerType == Type.PAWN) {
			return DeltaX == DeltaY && ((DeltaX == SIMPLE_MOVEMENT && boardManager.WasRemovedPiece == false && MovementValidationForTypeIsOk(From, To)) || DeltaX == FIGHT_MOVEMENT);
		} else {
			return DeltaX == DeltaY;
		}
	}
		
	private Boolean MovementValidationForTypeIsOk(Position From, Position To) {
		return Math.Sign(To.Y - From.Y) == (int) this.color; //in case of White checker deltaY should be positive (because we're moving from 1 to 8 rows), in case of black - should be negative
	}

	private Boolean isEnemyChecker(Position checkerPosition) {
		return this.color != boardManager.GetPiece(checkerPosition).Color;
	}

	private Boolean isFightMovement(Position From, Position To, List<Position> pieceBetweenPositions) {
		if (pieceBetweenPositions.Count != MAX_VALID_CHECKERS_COUNT_BETWEEN_POSITIONS) {
			return false;
		}
		if (this.CheckerType == Type.PAWN) {
			return Math.Abs (From.X - To.X) == FIGHT_MOVEMENT;
		} else {
			return Math.Abs (From.X - To.X) > SIMPLE_MOVEMENT;
		}
	}

	private void performFightMovement(Position From, Position To, List<Position> diagonalPieces) {
		Position eatenChecker = diagonalPieces.ElementAt (0);
		Debug.Log ("Should be eaten checker with pos " + eatenChecker);
		if (isBecomeAKing(To)) {
			this.checkerType = Type.KING;
		}
		boardManager.MoveChecker (this, From, To);
		boardManager.RemoveChecker (eatenChecker);
	}

	private void performSimpleMovement(Position From, Position To) {
		if (isBecomeAKing(To)) {
			this.checkerType = Type.KING;
		}
		boardManager.MoveChecker (this, From, To);
	}

	private Boolean isBecomeAKing(Position To) {
		return ((this.checkerType == Type.PAWN) && ((this.color == Color.WHITE && To.Y == 7) || (this.Color == Color.BLACK && To.Y == 0)));
	}
}
