using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 頂点の集まった平面ポリゴンクラス
/// (頂点は右回りで格納されていることが前提条件)
/// </summary>
public class Polygon2 {

	public Vector2[] vertices;	//頂点リスト
	public Edge2[] edges;		//辺リスト

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

		//辺の作成
		this.edges = CreateEdges(this.vertices);
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
		if (sumTheta < -Mathf.PI + 0.001f) {
			Debug.Log(sumTheta + " : CW");
			return true;
		} else if (sumTheta > Mathf.PI - 0.001f) {
			Debug.Log(sumTheta + " : CCW");
			return false;
		}

		//どちらでもない(エラーケース?)
		Debug.LogError("Argument vertices is error");
		return false;
	}

	/// <summary>
	/// 辺を作成する
	/// </summary>
	private Edge2[] CreateEdges(Vector2[] vertices) {
		
		//下準備
		int len = vertices.Length;
		Edge2[] edges = new Edge2[len];

		//処理
		for (int i = 0; i < len; ++i) {
			edges[i] = new Edge2(vertices[i], vertices[(i + 1) % len]);
		}
		return edges;
	}

	/// <summary>
	/// 辺をy座標値で分割
	/// 座標値は上から下にソートしてあること
	/// </summary>
	public List<Edge2> DivisionEdges(Edge2[] edges, float[] divisions) {

		//下準備
		List<Edge2> divEdges = new List<Edge2>();	//分割された辺
		
		//処理
		foreach(var e in edges) {
			List<Edge2> temp = DivisionEdge(e, divisions);
			divEdges.AddRange(temp);
		}

		return divEdges;
	}

	/// <summary>
	/// 辺をy座標値で分割
	/// 分割座標値は上から下にソートしてあること
	/// </summary>
	public List<Edge2> DivisionEdge(Edge2 edge, float[] divisions) {
		
		//下準備
		List<Edge2> edges = new List<Edge2>();
		Edge2 targetEdge = edge;	//対象

		//処理
		for (int i = 0; i < divisions.Length; ++i) {
			int contains = targetEdge.ContainsY(divisions[i]);
			if (contains == 1) {
				Edge2 a, b;
				if (targetEdge.Division(divisions[i], out a, out b)) {
					edges.Add(a);
					targetEdge = b;
				}
			}
		}

		//分割完了
		edges.Add(targetEdge);

		return edges;
	}

	/// <summary>
	/// 分割値を求める
	/// </summary>
	public float[] GetDivisions(Edge2[] aEdges, Edge2[] bEdges) {
		
		//下準備
		HashSet<float> divisionSet = new HashSet<float>();	//分割値の重複を防ぐ

		//座標値の追加
		//頂点の追加
		foreach (var e in aEdges) {
			if(divisionSet.Contains(e.from.y)) continue;
			divisionSet.Add(e.from.y);
		}
		foreach (var e in bEdges) {
			if(divisionSet.Contains(e.from.y)) continue;
			divisionSet.Add(e.from.y);
		}

		//交点の追加
		Vector2 p;
		foreach (var e in aEdges) {
			foreach (var f in bEdges) {
				if (!e.Intersection(f, out p)) continue;
				if (divisionSet.Contains(p.y)) continue;
				Debug.Log("Intersection : " + p.y);
				divisionSet.Add(p.y);
			}
		}

		//配列にしてソート
		float[] divisions = divisionSet.ToArray();
		Array.Sort(divisions);
		return divisions;
	}

	#endregion

	#region BooleanFunction

	/// <summary>
	/// 和
	/// </summary>
	public void Or(Polygon2 a) {

		//分割値を求める
		float[] divisions = GetDivisions(this.edges, a.edges);
		Debug.Log("Divisions");
		foreach (var e in divisions) {
			Debug.Log(e);
		}
	}

	#endregion
}