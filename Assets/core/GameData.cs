using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameData {

	private Piece[,] gameBoard;
	private Color whoGoes;
	private Position selectedPiece;
	private bool wasEatenEnemyPiece;
	private List<IAchievement> achievements;

	public GameData() {
		gameBoard = new Piece[0, 0];
		whoGoes = Color.WHITE;
		selectedPiece = null;
		wasEatenEnemyPiece = false;
		achievements = new List<IAchievement> ();
		achievements.Add (new WinCounter ());
	}
		
	public Piece[,] GameBoard {
		get {
			return this.gameBoard;
		}
		set {
			gameBoard = value;
		}
	}

	public Color WhoGoes {
		get {
			return this.whoGoes;
		}
		set {
			whoGoes = value;
		}
	}

	public Position SelectedPiece {
		get {
			return this.selectedPiece;
		}
		set {
			selectedPiece = value;
		}
	}

	public bool WasEatenEnemyPiece {
		get {
			return this.wasEatenEnemyPiece;
		}
		set {
			wasEatenEnemyPiece = value;
		}
	}

	public List<IAchievement> Achievements {
		get {
			return this.achievements;
		}
		set {
			achievements = value;
		}
	}
}
