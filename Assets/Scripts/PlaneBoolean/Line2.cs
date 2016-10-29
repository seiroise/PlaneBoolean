using UnityEngine;
using System.Collections;

/// <summary>
/// 平面直線
/// </summary>
public class Line2 {

	public float a;
	public float b;
	public float c;

	#region Constructors

	public Line2(float a, float b, float c) {
		this.a = a;
		this.b = b;
		this.c = c;
	}

	#endregion

	#region Function

	/// <summary>
	/// 直線との交点を求める
	/// </summary>
	public bool Intersection(Line2 l, out Vector2 p) {
		p = Vector2.zero;
		float d = a * l.b - l.a * b;
		if (d == 0f) {
			//平行
			return false;
		}
		float x = (b * l.c - l.b * c) / d;
		float y = (l.a * c - a * l.c) / d;
		p = new Vector2(x, y);
		return true;

	}

	#endregion

	#region StaticFunction

	/// <summary>
	/// 二点を通る直線を求める
	/// </summary>
	public static Line2 FromPoints(float x1, float y1, float x2, float y2) {
		float dx = x2 - x1;
		float dy = y2 - y1;
		return new Line2(dy, -dx, dx * y1 - dy * x1);
	}

	/// <summary>
	/// 二点を通る直線を求める
	/// </summary>
	public static Line2 FromPoints(Vector2 p1, Vector2 p2) {
		float dx = p2.x - p1.x;
		float dy = p2.y - p1.y;
		return new Line2(dy, -dx, dx * p1.y - dy * p1.x);
	}

	#endregion
}