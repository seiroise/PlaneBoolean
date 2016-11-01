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

		Rect plotRect = new Rect(-5f, -5f, 10f, 10f);

		//適当に頂点を追加
		List<Vector2f> inputVertices = new List<Vector2f>();
		int count = 10;
		for (int i = 0; i < count; ++i) {
			inputVertices.Add(new Vector2f(
				Random.Range(plotRect.xMin, plotRect.xMax),
				Random.Range(plotRect.yMin, plotRect.yMax)
			));
		}

		//ボロノイ図の作成
		voronoi = new Voronoi(inputVertices, new Rectf(plotRect.xMin, plotRect.yMin, plotRect.width, plotRect.height));

		//生成した範囲を取得
		List<List<Vector2f>> regions = voronoi.Regions();

		//論理演算可能なポリゴンに変換
		for (int i = 0; i < regions.Count; ++i) {
			Vector2[] vertices = new Vector2[regions[i].Count];
			//ブーリアンオブジェクト?の生成

		}

	}

	#endregion
}