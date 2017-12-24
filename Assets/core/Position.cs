using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Position {

	private int x;
	private int y;

	public int X {
		get {
			return this.x;
		}
		set {
			x = value;
		}
	}

	public int Y {
		get {
			return this.y;
		}
		set {
			y = value;
		}
	}

	public Position (int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public override string ToString ()
	{
		return string.Format ("[Cell: x={0}, y={1}]", x, y);
	}

	public override bool Equals (object obj)
	{
		if (obj == null)
			return false;
		if (ReferenceEquals (this, obj))
			return true;
		if (obj.GetType () != typeof(Position))
			return false;
		Position other = (Position)obj;
		return x == other.x && y == other.y;
	}
	

	public override int GetHashCode ()
	{
		unchecked {
			return x.GetHashCode () ^ y.GetHashCode ();
		}
	}
	
}
