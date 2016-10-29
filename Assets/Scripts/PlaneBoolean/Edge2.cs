using UnityEngine;
using System.Collections;

/// <summary>
/// 平面辺クラス
/// </summary>
public class Edge2 {

	public readonly Vector2 from;
	public readonly Vector2 to;
	public readonly Vector2 delta;

	public readonly float top;
	public readonly float bottom;

	public readonly LineSegment2 lineSeg;

	public readonly bool isHorizontal;	//水平
	public readonly bool isUpward;		//上向き
	public readonly bool isDownward;	//下向き

	public Edge2(Vector2 from, Vector2 to) {
		this.from = from;
		this.to = to;
		this.delta = to - from;

		this.lineSeg = new LineSegment2(from, to);

		//向き
		if (delta.y > 0f) {
			//上向き
			this.isUpward = true;
			this.top = to.y;
			this.bottom = from.y;
		} else if (delta.y < 0f) {
			//下向き
			this.isDownward = true;
			this.top = from.y;
			this.bottom = to.y;
		} else {
			//水平
			this.isHorizontal = true;
			this.top = this.bottom = to.y;
		}
	}

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
	public bool Division(float y, out Edge2 a, out Edge2 b) {
		a = b = null;
		if (isHorizontal) {
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
				a = new Edge2(from, p);
				b = new Edge2(p, to);
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
}