using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum Color
{
	WHITE = 1, BLACK = -1, NONE = 0
}
public enum Type {
	PAWN,
	KING,
	NONE
}
public class Piece : MonoBehaviour {

	private static readonly int SIMPLE_MOVEMENT = 1;
	private static readonly int FIGHT_MOVEMENT = 2;
	private static readonly int MAX_VALID_CHECKERS_COUNT_BETWEEN_POSITIONS = 1;

	public GameObject Board;

	private Board GameBoard;

	public Color Color;
	public Type CheckerType;

	private Position currentPosition;

	public Position CurrentPosition {
		get {
			return this.currentPosition;
		}
		set {
			currentPosition = value;
		}
	}

	public void Start() {
		GameBoard = Board.GetComponent<Board> ();
	}

	public Piece(Color color, Type type, Position currentPosition) {
		this.Color = color;
		this.CheckerType = type;
		this.currentPosition = currentPosition;
	}

	public void doMovement(Position To) {
		Position From = this.CurrentPosition;
		if (!isValidMovement (From, To)) {
			return;
		}
		if (isFightMovement (From, To)) {
			performFightMovement (From, To);
		} else {
			performSimpleMovement (From, To);
		}
	}

	private Boolean isValidMovement(Position From, Position To) {
		return basicValidationIsOk (From, To) && isValidCheckersCountBetween (GameBoard.GetCheckersBetween (From, To), From, To);
	}

	private Boolean basicValidationIsOk(Position From, Position To) {
		int DeltaX = Math.Abs (From.X - To.X);
		int DeltaY = Math.Abs (From.Y - To.Y);
		if (this.CheckerType == Type.PAWN) {
			return DeltaX == DeltaY && ((DeltaX == SIMPLE_MOVEMENT && MovementValidationForTypeIsOk(From, To)) || DeltaX == FIGHT_MOVEMENT);
		} else {
			return DeltaX == DeltaY;
		}
	}
		
	private Boolean MovementValidationForTypeIsOk(Position From, Position To) {
		return From.Y - To.Y == (int) this.Color; //in case of White checker deltaY should be positive (because we're moving from 1 to 8 rows), in case of black - should be negative
	}

	private Boolean isValidCheckersCountBetween(List<Piece> checkersBetween, Position From, Position To) {
		if (checkersBetween.Count > MAX_VALID_CHECKERS_COUNT_BETWEEN_POSITIONS) {
			return false;
		}
		if (checkersBetween.Count == MAX_VALID_CHECKERS_COUNT_BETWEEN_POSITIONS) {
			return isEnemyChecker (checkersBetween.ElementAt (0));
		}
		return false;
	}

	private Boolean isEnemyChecker(Piece checker) {
		return this.Color != checker.Color;
	}

	private Boolean isFightMovement(Position From, Position To) {
		return (From.X - To.X) == FIGHT_MOVEMENT;
	}

	private void performFightMovement(Position From, Position To) {
		Piece eatenChecker = GameBoard.GetCheckersBetween (From, To).ElementAt (0);
		GameBoard.RemoveChecker (eatenChecker);
		GameBoard.MoveChecker (this, To);
	}

	private void performSimpleMovement(Position From, Position To) {
		GameBoard.MoveChecker (this, To);
		if(isBecomeAKing(To)){
			this.CheckerType = Type.KING;
		}
	}

	private Boolean isBecomeAKing(Position To) {
		return ((this.CheckerType == Type.PAWN) && ((this.Color == Color.WHITE && To.Y == 8) || (this.Color == Color.BLACK && To.Y == 1)));
	}
}
