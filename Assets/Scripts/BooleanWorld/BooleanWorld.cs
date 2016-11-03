using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Seiro.Scripts.Geometric.Diagram.Voronoi;
using Seiro.Scripts.Geometric.Polygon.Convex;
using csDelaunay;

/// <summary>
/// 論理演算可能な世界
/// </summary>
public class BooleanWorld : MonoBehaviour {

	//取り敢えずボロノイ図から作ってみる
	private Voronoi voronoi;

	public BooleanObject objectPrefab;

	#region UnityEvent

	private void Start() {
		GenerateWorld();
	}

	#endregion

	#region Function

	/// <summary>
	/// 世界の生成
	/// </summary>
	private void GenerateWorld() {

		float size = 20f;

		Rect plotRect = new Rect(-size * 0.5f, -size * 0.5f, size, size);

		//適当にサイトを作成
		List<Vector2f> sites = new List<Vector2f>();
		int count = 10;
		for (int i = 0; i < count; ++i) {
			sites.Add(new Vector2f(
				Random.Range(plotRect.xMin, plotRect.xMax),
				Random.Range(plotRect.yMin, plotRect.yMax)
			));
		}

		//ボロノイ図の作成
		voronoi = new Voronoi(sites, new Rectf(plotRect.xMin, plotRect.yMin, plotRect.width, plotRect.height));

		//生成した範囲を取得
		List<List<Vector2f>> regions = voronoi.Regions();

		//論理演算可能なポリゴンに変換
		for (int i = 0; i < regions.Count; ++i) {
			List<Vector2f> r = regions[i];
			Vector2[] vertices = new Vector2[r.Count];
			Vector2 site = new Vector2(sites[i].x, sites[i].y);

			for (int j = 0; j < vertices.Length; ++j) vertices[j] = new Vector2(r[j].x, r[j].y);
			//ブーリアンオブジェクト?の生成
			InstantiateBooleanObject(vertices);
		}
	}

	/// <summary>
	/// ブーリアンオブジェクトの生成
	/// </summary>
	private BooleanObject InstantiateBooleanObject(Vector2[] vertices) {
		BooleanObject obj = Instantiate<BooleanObject>(objectPrefab);
		obj.SetVertices(vertices);
		obj.transform.SetParent(transform, false);
		return obj;
	}

	#endregion
}