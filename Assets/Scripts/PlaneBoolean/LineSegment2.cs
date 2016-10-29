using UnityEngine;
using System.Collections;

/// <summary>
/// 平面線分
/// </summary>
public class LineSegment2 {

	public Vector2 p1;
	public Vector2 p2;

	#region Constructors

	public LineSegment2(Vector2 p1, Vector2 p2) {
		this.p1 = p1;
		this.p2 = p2;
	}

	#endregion

	#region Function

	/// <summary>
	/// 二次元直線に変換
	/// </summary>
	public Line2 ToLine() {
		return Line2.FromPoints(p1, p2);
	}

	/// <summary>
	/// 二次元直線との交差判定
	/// </summary>
	public bool Intersection(Line2 l) {
		float t1 = l.a * p1.x + l.b * p1.y + l.c;   //端点1の座標を直線の式に代入
		float t2 = l.a * p2.x + l.b * p2.y + l.c;   //端点2の座標を直線の式に代入

		return t1 * t2 <= 0;    //判定
	}

	/// <summary>
	/// 二次元線分との交差判定
	/// (包含判定は未実装)
	/// </summary>
	public bool Intersection(LineSegment2 s) {
		//２つの線分から見て互いが両側に居るか確認
		return HostSides(s) && s.HostSides(this);
	}

	/// <summary>
	/// 二次元直線との交点を求める
	/// </summary>
	public bool Intersection(Line2 l, out Vector2 p) {
		p = Vector2.zero;
		if (!Intersection(l)) {
			return false;   //交差しない場合はfalseを返す
		}
		return l.Intersection(ToLine(), out p);
	}

	/// <summary>
	/// 二次元線分との交点を求める
	/// </summary>
	public bool Intersection(LineSegment2 s, out Vector2 p) {
		p = Vector2.zero;
		if (!Intersection(s)) {
			return false;   //交差しない場合はfalseを返す
		}
		return s.ToLine().Intersection(ToLine(), out p);
	}

	/// <summary>
	/// sが自線分の「両側」にあるかどうかを調べる
	/// </summary>
	public bool HostSides(LineSegment2 s) {
		float ccw1 = GeomUtil.CCW(p1, s.p1, p2);
		float ccw2 = GeomUtil.CCW(p1, s.p2, p2);

		if (ccw1 == 0f && ccw2 == 0f) {
			//sと自線分が一直線上にある場合
			//sのずれか1つの端点が自線分を内分していれば,sは自線分と共有部分を持ちtrueを返す
			return Internal(s.p1) || Internal(s.p2);
		} else {
			//それ以外
			//CCW値の富豪が異なる場合にtrueを返す
			return ccw1 * ccw2 < 0f;
		}
	}

	/// <summary>
	/// 点pが自線分を内分しているかどうかを調べる
	/// </summary>
	public bool Internal(Vector2 p) {
		//pから端点に向かうベクトルの内積が0以下てあれば内分とみなす
		return GeomUtil.Dot(p1 - p, p2 - p) <= 0;
	}

	#endregion
}