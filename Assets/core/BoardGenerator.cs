using System.Collections;
using System.Collections.Generic;

public class BoardGenerator {

	public Piece[,] GenerateStartGameBoard(BoardManager boardManager) {
		Piece[,] gameBoard = new Piece[8, 8];

		//gen white pieces
		for (int y = 0; y < 3; y++) {
			generateLine (gameBoard, y, Color.WHITE, boardManager);
		}

		//gen black pieces
		for (int y = 5; y < 8; y++) {
			generateLine (gameBoard, y, Color.BLACK, boardManager);
		}

		return gameBoard;
	}

	public Piece[,] GenerateStartGameBoard(BoardManager boardManager, Piece[,] preGeneratedBoard) {
		for (int y = 0; y < 8; y++) {
			int start = (y % 2 == 1) ? 1 : 0;
			for (int x = start; x < 8; x += 2) {
				if (!object.ReferenceEquals (null, preGeneratedBoard [x, y])) {
					preGeneratedBoard [x, y].BoardManager = boardManager;
				}
			}
		}
		return preGeneratedBoard;
	}

	private void generateLine(Piece[,] gameBoard, int lineNumber, Color color, BoardManager boardManager) {
		int start = (lineNumber % 2 == 1) ? 1 : 0;
		for (int x = start; x < 8; x += 2) {
			gameBoard [x, lineNumber] = GeneratePiece(color, Type.PAWN, boardManager);
		}
	}

	private Piece GeneratePiece (Color color, Type type, BoardManager boardManager) {
		return new Piece (color, type, boardManager);
	}

}
