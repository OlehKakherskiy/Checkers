using System.Collections;
using System.Collections.Generic;

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
}
