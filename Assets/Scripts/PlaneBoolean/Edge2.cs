using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 平面辺クラス
/// </summary>
public class Edge2 : IComparable<Edge2> {

	public enum Direction {
		Horizontal,
		Upward,
		Downward
	}

	public readonly Vector2 from;
	public readonly Vector2 to;
	public readonly Vector2 delta;

	public readonly float top;
	public readonly float bottom;
	public readonly float left;
	public readonly float right;

	public readonly LineSegment2 lineSeg;

	public readonly Direction deltaDir;

	public int counter;

	#region Constructors

	public Edge2(Vector2 from, Vector2 to) {
		this.from = from;
		this.to = to;
		this.delta = to - from;

		this.lineSeg = new LineSegment2(from, to);

		//向き
		if (delta.y > 0f) {
			//上向き
			this.deltaDir = Direction.Upward;
			this.top = to.y;
			this.bottom = from.y;
		} else if (delta.y < 0f) {
			//下向き
			this.deltaDir = Direction.Downward;
			this.top = from.y;
			this.bottom = to.y;
		} else {
			//水平
			this.deltaDir = Direction.Horizontal;
			this.top = this.bottom = to.y;
		}

		if (delta.x > 0f) {
			this.left = from.x;
			this.right = to.x;
		} else if (delta.x < 0f) {
			this.left = to.x;
			this.right = from.x;
		} else {
			this.left = this.right = from.x;
		}

		counter = 0;
	}

	#endregion

	#region VirtualFunction

	public override string ToString() {
		StringBuilder sb = new StringBuilder();
		sb.Append(from);
		sb.Append(" -> ");
		sb.Append(to);
		sb.Append(" : ");
		sb.Append(deltaDir);
		sb.Append(" : ");
		sb.Append(counter);

		return sb.ToString();
	}

	#endregion

	#region Function

	/// <summary>
	/// y座標の包含判定
	/// 含むなら1,含まないなら-1,点上なら0
	/// </summary>
	public int ContainsY(float y) {
		if (top > y && y > bottom) {
			return 1;
		} else if (y == top || y == bottom) {
			return 0;
		} else {
			return -1;
		}
	}

	/// <summary>
	/// y軸で分割する。分割したならtrueを返す
	/// </summary>
	public bool Division(float y, out Edge2 top, out Edge2 bottom) {
		top = bottom = null;
		if (deltaDir == Direction.Horizontal) {
			//水平な場合
			return false;
		} else {
			//水平以外
			
			//判定する線分直線を作成
			LineSegment2 s = new LineSegment2(from, to);
			Line2 l = Line2.FromPoints(0, y, 1, y);

			//交点を求める
			Vector2 p;
			if (s.Intersection(l, out p)) {
				//分割した辺を作成
				if (deltaDir == Direction.Upward) {
					top = new Edge2(p, to);
					bottom = new Edge2(from, p);
				} else {
					top = new Edge2(from, p);
					bottom = new Edge2(p, to);
				}
				return true;
			} else {
				return false;
			}
		}
	}

	/// <summary>
	/// 辺同士の交点を求める
	/// </summary>
	public bool Intersection(Edge2 edge, out Vector2 p) {
		return lineSeg.Intersection(edge.lineSeg, out p);
	}

	#endregion

	#region IComparable

	public int CompareTo(Edge2 other) {
		if (this.left < other.left) {
			return -1;
		} else {
			return 1;
		}
	}

	#endregion
}