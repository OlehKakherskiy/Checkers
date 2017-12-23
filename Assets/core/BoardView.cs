using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BoardView : MonoBehaviour {
	public Sprite WhitePawn;
	public Sprite WhiteKing;
	public Sprite BlackPawn;
	public Sprite BlackKing;

	public void InitView(Piece[,] boardModel) {
		Debug.Log (boardModel);
		for (int y = 0; y < 8; y++) {
			for (int x = 0; x < 8; x++) {
				if (boardModel [x, y] != null) {
					UpdateCellView (boardModel [x, y], new Position (x, y));
				}
			}
		}
	}

	public void UpdateCellView(Piece piece, Position position) {
		GameObject panelToUpdateImage = findCellView (position.X, position.Y);
		Debug.Log (position);
		Debug.Log (panelToUpdateImage.name);
		Sprite imgToSet = null;
		if (piece.Color == Color.WHITE) {
			imgToSet = (piece.CheckerType == Type.PAWN) ? WhitePawn : WhiteKing;
		} else  {
			imgToSet = (piece.CheckerType == Type.PAWN) ? BlackPawn : BlackKing;
		}
		Image img = panelToUpdateImage.GetComponent<Image> ();
		img.sprite = imgToSet;
		changeColorTransparency (img, 255);
	}

	public void RemovePiece(Position position) {
		Debug.Log ("Disabling cell with name " + findCellView (position.X, position.Y).name);
		changeColorTransparency (findCellView (position.X, position.Y).GetComponent<Image> (), 0);
	}

	private GameObject findCellView(int x, int y) {
		return GameObject.Find ("Cell " + ((y * 8 + x) / 2));
	}

	private void changeColorTransparency(Image image, int transparency) {
		UnityEngine.Color c = image.color;
		c.a = transparency;
		image.color = c;
	}
}
