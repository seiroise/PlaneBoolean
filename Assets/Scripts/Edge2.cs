using UnityEngine;
using System.Collections;

/// <summary>
/// 平面辺クラス
/// </summary>
public class Edge2 {

	public readonly Vector2 from;
	public readonly Vector2 to;
	public readonly Vector2 delta;

	public readonly bool isHorizontal;	//水平
	public readonly bool isUpward;		//上向き
	public readonly bool isDownward;	//下向き

	public Edge2(Vector2 from, Vector2 to) {
		this.from = from;
		this.to = to;
		this.delta = to - from;

		//向き
		if (delta.y > 0f) {
			this.isUpward = true;
		} else if (delta.y < 0f) {
			this.isDownward = true;
		} else {
			this.isHorizontal = true;
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
			if (isUpward) {
				if (from.y < y && y < to.y) {
					
				}
			} else {
				
			}
		}
		
		return true;
	}
}