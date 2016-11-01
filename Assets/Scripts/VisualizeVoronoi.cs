using UnityEngine;
using System.Collections.Generic;
using csDelaunay;
using Seiro.Scripts.Geometric.Polygon.Convex;
using Seiro.Scripts.Graphics;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisualizeVoronoi : MonoBehaviour {

	private Voronoi voronoi;
	private MeshFilter mf;

	private void Start() {

		Rect plotRect = new Rect(-5f, -5f, 10f, 10f);

		//適当に頂点を追加
		List<Vector2f> inputVertices = new List<Vector2f>();
		int count = 10;
		for(int i = 0; i < count; ++i) {
			inputVertices.Add(new Vector2f(
				Random.Range(plotRect.xMin, plotRect.xMax),
				Random.Range(plotRect.yMin, plotRect.yMax)
			));
		}

		//ボロノイ図の作成
		voronoi = new Voronoi(inputVertices, new Rectf(plotRect.xMin, plotRect.yMin, plotRect.width, plotRect.height));

		//生成した範囲を取得
		List<List<Vector2f>> regions = voronoi.Regions();

		//画面に描画するためにメッシュに変換
		List<Vector2> vertices = new List<Vector2>();
		EasyMesh[] eMeshes = new EasyMesh[regions.Count];
		int k = 0;
		foreach(var e in regions) {
			if(e.Count < 3) {
				++k;
				continue;
			}
			vertices.Clear();
			foreach(var f in e) {
				vertices.Add(new Vector2(f.x, f.y));
			}
			ConvexPolygon poly = new ConvexPolygon(vertices);
			eMeshes[k] = poly.ToEasyMesh(new Color(
				Random.Range(0f, 1f),
				Random.Range(0f, 1f),
				Random.Range(0f, 1f)
			));
			++k;
		}

		//メッシュに変換して描画
		Mesh mesh = EasyMesh.ToMesh(eMeshes);
		GetComponent<MeshFilter>().mesh = mesh;
	}

	private void Update() {
		if(Input.GetMouseButtonDown(0)) {
			Start();
		}
	}
}