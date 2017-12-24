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
public class Piece {

	private static readonly int SIMPLE_MOVEMENT = 1;
	private static readonly int FIGHT_MOVEMENT = 2;
	private static readonly int MAX_VALID_CHECKERS_COUNT_BETWEEN_POSITIONS = 1;

	private Color color;
	private Type checkerType;
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

	public Piece(Color color, Type type, BoardManager boardManager) {
		this.color = color;
		this.checkerType = type;
		this.boardManager = boardManager;
	}

	public bool doMovement(Position From, Position To) {
		if (!isValidMovement (From, To)) {
			return false;
		}
		if (isFightMovement (From, To)) {
			performFightMovement (From, To);
			boardManager.WasRemovedPiece = true;
		} else {
			performSimpleMovement (From, To);
			boardManager.WasRemovedPiece = false;
		}
		return true;
	}

	private Boolean isValidMovement(Position From, Position To) {
		return basicValidationIsOk (From, To) && isValidCheckersCountBetween (boardManager.GetCheckersBetween (From, To), From, To);
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
		return To.Y - From.Y == (int) this.color; //in case of White checker deltaY should be positive (because we're moving from 1 to 8 rows), in case of black - should be negative
	}

	private Boolean isValidCheckersCountBetween(List<Position> checkersBetween, Position From, Position To) {
		checkersBetween.RemoveAll (position => object.ReferenceEquals(null, position));
		checkersBetween.RemoveAll (position => To.Equals (position));
		if (checkersBetween.Count > MAX_VALID_CHECKERS_COUNT_BETWEEN_POSITIONS) {
			return false;
		}
		bool fightMovement = isFightMovement (From, To);
		if (fightMovement && checkersBetween.Count == 0) {
			return false;
		}
		if (checkersBetween.Count == MAX_VALID_CHECKERS_COUNT_BETWEEN_POSITIONS) {
			return isEnemyChecker (checkersBetween.ElementAt (0));
		}
		return true;
	}

	private Boolean isEnemyChecker(Position checkerPosition) {
		return this.color != boardManager.GetPiece(checkerPosition).Color;
	}

	private Boolean isFightMovement(Position From, Position To) {
		return (this.CheckerType == Type.PAWN && Math.Abs(From.X - To.X) == FIGHT_MOVEMENT) || Math.Abs(From.X - To.X) > SIMPLE_MOVEMENT;
	}

	private void performFightMovement(Position From, Position To) {
		List<Position> diagonalElements = boardManager.GetCheckersBetween (From, To);
		diagonalElements.RemoveAll(position => object.ReferenceEquals(null, position));
		Position eatenChecker = diagonalElements.ElementAt (0);
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
