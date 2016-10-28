using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 頂点の集まった平面ポリゴンクラス
/// (頂点は右回りで格納されていることが前提条件)
/// </summary>
public class Polygon2 {

	public Vector2[] vertices;	//頂点リスト
	public Edge2[] edges;		//辺リスト
	public Edge2[] topBottomEdges;	//時計回り(右回り)に上から下
	public Edge2[] bottomTopEdges;	//時計回り(左回り)に下から上

	private int topIndex;		//最上点のインデックス
	private int bottomIndex;	//最下点のインデックス

	#region Constructors

	public Polygon2(Vector2[] vertices) {

		//エラーチェック
		if (vertices == null || vertices.Length < 1) return;

		//回りの確認
		if (!CheckCW(vertices)) {
			//配列を反転
			Array.Reverse(vertices);
		}

		//頂点の設定
		this.vertices = vertices;

		//最上点と最下点を求める
		CheckTopBottom(this.vertices, out topIndex, out bottomIndex);

		//辺の作成
		topBottomEdges = CreateTopBottomEdges(this.vertices);
		bottomTopEdges = CreateBottomTopEdges(this.vertices);
	}

	#endregion

	#region Function

	/// <summary>
	/// 頂点の配列が時計回り(右回り)か確認する
	/// </summary>
	private bool CheckCW(Vector2[] vertices) {
		
		//角度の総和を求める
		int len = vertices.Length;
		float sumTheta = 0f;
		for (int i = 0; i < len; ++i) {
			
			//θを求める
			Vector2 p1, p2, p3;
			p1 = vertices[i];
			p2 = vertices[(i + 1) % len];
			p3 = vertices[(i + 2) % len];
			float theta1 = Mathf.Atan2(p2.y - p1.y, p2.x - p1.x);
			float theta2 = Mathf.Atan2(p3.y - p2.y, p3.x - p2.x);
			float theta = theta2 - theta1;

			//θが-π ~ πの範囲を超えないように調整
			if (theta > Mathf.PI) theta -= Mathf.PI;
			else if (theta < -Mathf.PI) theta += Mathf.PI;

			sumTheta += theta;
		}

		//回り方の判定
		if (-sumTheta < Mathf.PI + 0.001f) {
			return true;
		} else if (sumTheta > Mathf.PI - 0.001f) {
			return false;
		}

		//どちらでもない(エラーケース?)
		Debug.LogError("Argument vertices is error");
		return false;
	}

	/// <summary>
	/// 最上点と最下点を確認する
	/// </summary>
	private void CheckTopBottom(Vector2[] vertices, out int top, out int bottom) {
		
		//準備
		float min, max, y;
		top = bottom = 0;
		min = max = vertices[0].y;

		//最上点と最下点を求める
		for (int i = 1; i < vertices.Length; ++i) {
			y = vertices[i].y;
			if (y > max) {
				max = y;
				top = i;
			} else if (y < min) {
				min = y;
				bottom = i;
			}
		}
	}

	/// <summary>
	/// 時計回りに最上点から最下点を結ぶ辺を作成する
	/// </summary>
	private Edge2[] CreateTopBottomEdges(Vector2[] vertices) {

		//準備
		int i = topIndex;
		int len = vertices.Length;
		List<Edge2> edges = new List<Edge2>();

		//処理
		while (bottomIndex != i) {
			int next = (i + 1) % len;	//時計回りなのでインクリ
			edges.Add(new Edge2(vertices[i], vertices[next]));
			i = next;
		}

		return edges.ToArray();
	}

	/// <summary>
	/// 時計回りに最下点から最上点を結ぶ辺を作成する
	/// </summary>
	private Edge2[] CreateBottomTopEdges(Vector2[] vertices) {

		//準備
		int i = bottomIndex;
		int len = vertices.Length;
		List<Edge2> edges = new List<Edge2>();

		//処理
		while (topIndex != i) {
			int next = (i + len - 1) % len;	//反時計回りなのでデクリ
			edges.Add(new Edge2(vertices[i], vertices[next]));
			i = next;
		}
		return edges.ToArray();
	}

	/// <summary>
	/// 辺をy座標値で分割
	/// 座標値は上から下にソートしてあること
	/// </summary>
	public List<Edge2>[] DivisionEdges(float[] divisions) {

		//下準備
		int len = divisions.Length;
		List<Edge2>[] edges = new List<Edge2>[len - 1];

		//左側(topBottomEdges)
		Edge2 divEdge = topBottomEdges[0];	//分割対象の辺
		for (int i = 0; i < len; ++i) {
			if(divEdge)
		}

		return edges;
	}

	#endregion

	#region BooleanFunction

	/// <summary>
	/// 和
	/// </summary>
	public void Or(Polygon2 a) {
		
		

	}

	#endregion
}