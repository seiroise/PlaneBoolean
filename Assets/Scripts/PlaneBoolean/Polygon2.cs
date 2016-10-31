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
	/// 辺の向きを反転させる
	/// </summary>
	public Edge2[] ReverseEdges() {

		//下準備
		int len = vertices.Length;
		Edge2[] edges = new Edge2[len];

		//処理
		for (int i = 0; i < len; ++i) {
			edges[i] = new Edge2(vertices[i], vertices[(i + len - 1) % len]);
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
		foreach (var e in edges) {
			List<Edge2> temp = DivisionEdge(e, divisions);
			if (temp == null) continue;
			divEdges.AddRange(temp);
		}

		return divEdges;
	}

	/// <summary>
	/// 辺をy座標値で分割
	/// 座標値は上から下にソートしてあること
	/// </summary>
	public List<Edge2> DivisionEdges(float[] divisions) {
		return DivisionEdges(this.edges, divisions);
	}

	/// <summary>
	/// 辺をy座標値で分割
	/// 座標値は上から下にソートしてあること
	/// </summary>
	public List<Edge2> DivisionEdge(Edge2 edge, float[] divisions) {
		
		//平行な場合はそもそも分割できない
		if (edge.deltaDir == Edge2.Direction.Horizontal) return null;
		
		//下準備
		List<Edge2> edges = new List<Edge2>();
		Edge2 targetEdge = edge;	//対象

		//処理
		for (int i = 0; i < divisions.Length; ++i) {
			int contains = targetEdge.ContainsY(divisions[i]);
			if (contains == 1) {
				//上から下へ分割
				Edge2 top, bottom;
				if (targetEdge.Division(divisions[i], out top, out bottom)) {
					edges.Add(top);
					targetEdge = bottom;
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
				divisionSet.Add(p.y);
			}
		}

		//配列にしてソート(大きい順)
		float[] divisions = divisionSet.ToArray();
		Array.Sort(divisions, (x, y) => {
			if (x > y) return -1;
			else if (x < y) return 1;
			else return 0;
		});

		return divisions;
	}

	/// <summary>
	/// 分割値毎に辺を集める
	/// </summary>
	public List<Edge2>[] CollectEdges(List<Edge2> edges, float[] divisions) {

		if (edges == null || edges.Count == 0) return null;

		//下準備
		int edgeLen = edges.Count;
		int divLen = divisions.Length - 1;
		Edge2 edge;
		List<Edge2>[] divEdges = new List<Edge2>[divLen];

		//処理
		for (int i = 0; i < edgeLen; ++i) {
			edge = edges[i];
			for (int j = 0; j < divLen; ++j) {
				if (edge.top == divisions[j]) {
					if (divEdges[j] == null) divEdges[j] = new List<Edge2>();
					divEdges[j].Add(edge);
					break;
				}
			}
		}

		//辺を左順にソートしカウンタを載せる
		for (int i = 0; i < divLen; ++i) {
			List<Edge2> e = divEdges[i];
			if (e == null) continue;
			e.Sort();
			int counter = 0;
			foreach (var f in e) {
				if (f.deltaDir == Edge2.Direction.Upward) {
					counter++;
				} else {
					counter--;
				}
				f.counter = counter;
			}
		}

		return divEdges;
	}

	#endregion

	#region BooleanFunction

	/// <summary>
	/// 論理和
	/// </summary>
	public Mesh Or(Polygon2 a) {

		//分割値を求める
		float[] divisions = GetDivisions(this.edges, a.edges);
		//辺の分割
		List<Edge2> divEdges = DivisionEdges(divisions);
		List<Edge2> aDiv = a.DivisionEdges(divisions);
		divEdges.AddRange(aDiv);

		//分割した辺を分割値毎にまとめ左順にソート
		List<Edge2>[] collectEdges = CollectEdges(divEdges, divisions);

		//メッシュ化
		Mesh mesh = new Mesh();
		List<Vector3> vertices = new List<Vector3>();
		List<int> indices = new List<int>();
		int index = 0;

		//左から順に上向きのカウンタ1の辺と下向きのカウンタ0の辺をペアとする
		foreach (var edges in collectEdges) {
			Edge2 edge = null;
			foreach (var e in edges) {
				if (edge == null) {
					if (e.deltaDir == Edge2.Direction.Upward && e.counter == 1) {
						edge = e;
					}
				} else {
					if (e.deltaDir == Edge2.Direction.Downward && e.counter == 0) {
						if (edge.from == e.to) {
							//Triangleの作成
							vertices.Add(edge.from);
							vertices.Add(edge.to);
							vertices.Add(e.from);

							indices.Add(index++);
							indices.Add(index++);
							indices.Add(index++);
						}else if(edge.to == e.from) {
							//Triangleの作成
							vertices.Add(edge.from);
							vertices.Add(edge.to);
							vertices.Add(e.to);

							indices.Add(index++);
							indices.Add(index++);
							indices.Add(index++);
						} else {
							//Quadの作成
							vertices.Add(edge.from);
							vertices.Add(edge.to);
							vertices.Add(e.from);
							vertices.Add(e.to);

							indices.Add(index);
							indices.Add(index + 1);
							indices.Add(index + 2);

							indices.Add(index);
							indices.Add(index + 2);
							indices.Add(index + 3);

							index += 4;
						}
						
						edge = null;
					}
				}
			}
		}

		mesh.SetVertices(vertices);
		mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		return mesh;
	}

	/// <summary>
	/// 論理否定
	/// </summary>
	public Mesh Not(Polygon2 a) {

		//分割値を求める
		float[] divisions = GetDivisions(this.edges, a.edges);
		//辺の分割
		List<Edge2> divEdges = DivisionEdges(divisions);
		List<Edge2> aDiv = a.DivisionEdges(a.ReverseEdges(), divisions);	//辺を反転
		divEdges.AddRange(aDiv);

		//分割した辺を分割値毎にまとめ左順にソート
		List<Edge2>[] collectEdges = CollectEdges(divEdges, divisions);

		//メッシュ化
		Mesh mesh = new Mesh();
		List<Vector3> vertices = new List<Vector3>();
		List<int> indices = new List<int>();
		int index = 0;

		//左から順に上向きのカウンタ1の辺と下向きのカウンタ0の辺をペアとする
		foreach (var edges in collectEdges) {
			Edge2 edge = null;
			foreach (var e in edges) {
				if (edge == null) {
					if (e.deltaDir == Edge2.Direction.Upward && e.counter == 1) {
						edge = e;
					}
				} else {
					if (e.deltaDir == Edge2.Direction.Downward && e.counter == 0) {
						if (edge.from == e.to) {
							//Triangleの作成
							vertices.Add(edge.from);
							vertices.Add(edge.to);
							vertices.Add(e.from);

							indices.Add(index++);
							indices.Add(index++);
							indices.Add(index++);
						} else if (edge.to == e.from) {
							//Triangleの作成
							vertices.Add(edge.from);
							vertices.Add(edge.to);
							vertices.Add(e.to);

							indices.Add(index++);
							indices.Add(index++);
							indices.Add(index++);
						} else {
							//Quadの作成
							vertices.Add(edge.from);
							vertices.Add(edge.to);
							vertices.Add(e.from);
							vertices.Add(e.to);

							indices.Add(index);
							indices.Add(index + 1);
							indices.Add(index + 2);

							indices.Add(index);
							indices.Add(index + 2);
							indices.Add(index + 3);

							index += 4;
						}

						edge = null;
					}
				}
			}
		}

		mesh.SetVertices(vertices);
		mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		return mesh;
	}

	#endregion
}