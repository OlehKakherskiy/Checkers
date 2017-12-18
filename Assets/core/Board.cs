using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour {

	private Piece[,] board = new Piece[8,8];
	private GameObject[,] boardView = new GameObject[8, 8];

	public Sprite whitePawn;
	public Sprite whiteKing;
	public Sprite blackPawn;
	public Sprite blackKing;

	// Use this for initialization
	void Start () {
		GenerateBoard ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void GenerateBoard() {
		//gen white pieces
		for (int y = 0; y < 3; y++) {
			generateLine (y, Color.WHITE);
		}
		generateLine (3, Color.NONE);
		generateLine (4, Color.NONE);
		for (int y = 5; y < 8; y++) {
			generateLine (y, Color.BLACK);
		}
	}

	private void generateLine(int lineNumber, Color color) {
		int start = (lineNumber % 2 == 1) ? 1 : 0;
		int counter = 0;
		for (int x = start; x < 8; x += 2) {
			Debug.Log ("x=" + x + ", y=" + lineNumber);
			Debug.Log ("taking cell N = " + (counter + lineNumber * 4));
			//clearing and preparing cell
			boardView [x, lineNumber] = GameObject.Find ("Cell " + (counter + lineNumber * 4));
			boardView [x, lineNumber].SetActive (false);
			counter++;
			if (color != Color.NONE) {
				Piece p = GeneratePiece (x, lineNumber, color);
				MoveChecker(p, new Position(x, lineNumber));
			}
		}
	}

	private Piece GeneratePiece (int x, int y, Color color)
	{
		Piece p = findCellView(x, y).GetComponent<Piece> ();
		p.CheckerType = Type.PAWN;
		p.Color = color;
		return p;
	}

	public void MoveChecker(Piece piece, Position To){
		piece.CurrentPosition = To;
		board [To.X, To.Y] = piece;
		setPanelImage (piece, To);
	}

	public void RemoveChecker(Piece pieceToRemove) {
	}

	public List<Piece> GetCheckersBetween(Position From, Position To){
		return null;
	}

	private GameObject findCellView(int x, int y) {
		
		return boardView [x, y];
	}

	private void setPanelImage(Piece piece, Position To) {
		GameObject panelToUpdateImage = findCellView (To.X, To.Y);
		Sprite imgToSet = null;
		if (piece.Color == Color.WHITE) {
			imgToSet = (piece.CheckerType == Type.PAWN) ? whitePawn : whiteKing;
		} else {
			imgToSet = (piece.CheckerType == Type.PAWN) ? blackPawn : blackKing;
		}
		panelToUpdateImage.GetComponent<Image> ().sprite = imgToSet;
		panelToUpdateImage.SetActive (true);
		//findCellView (piece.CurrentPosition.X, piece.CurrentPosition.Y).SetActive (false);
	}
}
