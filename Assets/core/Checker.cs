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
public class Checker : MonoBehaviour {

	private static readonly int SIMPLE_MOVEMENT = 1;
	private static readonly int FIGHT_MOVEMENT = 2;
	private static readonly int MAX_VALID_CHECKERS_COUNT_BETWEEN_POSITIONS = 1;

	Board GameBoard;

	private Color Color;
	private Type CheckerType;

	private Position currentPosition;

	public Position CurrentPosition {
		get {
			return this.currentPosition;
		}
		set {
			currentPosition = value;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
		return From.Y - To.Y == this.Color; //in case of White checker deltaY should be positive (because we're moving from 1 to 8 rows), in case of black - should be negative
	}

	private Boolean isValidCheckersCountBetween(List<Checker> checkersBetween, Position From, Position To) {
		if (checkersBetween.Count > MAX_VALID_CHECKERS_COUNT_BETWEEN_POSITIONS) {
			return false;
		}
		if (checkersBetween.Count == MAX_VALID_CHECKERS_COUNT_BETWEEN_POSITIONS) {
			return isEnemyChecker (checkersBetween.ElementAt (0));
		}
	}

	private Boolean isEnemyChecker(Checker checker) {
		return this.Color != checker.Color;
	}

	private Boolean isFightMovement(Position From, Position To) {
		return (From.X - To.X) == FIGHT_MOVEMENT;
	}

	private void performFightMovement(Position From, Position To) {
		Checker eatenChecker = GameBoard.GetCheckersBetween (From, To).ElementAt (0);
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
