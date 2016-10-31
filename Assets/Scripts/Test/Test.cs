using UnityEngine;
using System.Linq;
using System.Collections;
using System.Diagnostics;

public class Test : MonoBehaviour {

	private Polygon2 a;
	private Polygon2 b;

	public LineRenderer aLine;
	public LineRenderer bLine;

	public MeshFilter orMesh;
	public MeshFilter notMesh;

	#region UnityEvent

	private void Start() {
		
		//テスト用頂点配列
		Vector2[] aVerts = new Vector2[] {
			new Vector2(0f, 0f),
			new Vector2(1f, 3f),
			new Vector2(4f, 3f),
			new Vector2(3f, 0f),
		};
		Vector2[] bVerts = new Vector2[] {
			new Vector2(2f, 2f),
			new Vector2(4f, 4f),
			new Vector2(6f, 2f),
			new Vector2(4f, 0f),
		};

		//線の描画
		SetLineRenderer(aLine, aVerts);
		SetLineRenderer(bLine, bVerts);

		//ポリゴンの生成
		a = new Polygon2(aVerts);
		b = new Polygon2(bVerts);

		//論理演算のテスト
		Stopwatch sw = Stopwatch.StartNew();
		a.Or(b);
		sw.Stop();
		UnityEngine.Debug.Log("Or : " + sw.Elapsed);

		sw.Reset();
		sw.Start();
		a.Not(b);
		sw.Stop();
		UnityEngine.Debug.Log("Not : " + sw.Elapsed);
	}

	private void Update() {
		if (Input.GetMouseButtonDown(0)) {
			//論理演算のテスト

			Stopwatch sw = Stopwatch.StartNew();
			a.Or(b);
			sw.Stop();
			UnityEngine.Debug.Log("Or : " + sw.ElapsedMilliseconds);

			sw.Reset();
			sw.Start();
			a.Not(b);
			sw.Stop();
			UnityEngine.Debug.Log("Not : " + sw.ElapsedMilliseconds);
		}
	}
	#endregion

	#region Function

	private void SetLineRenderer(LineRenderer line, Vector2[] vertices) {
		line.SetVertexCount(vertices.Length);
		for(int i = 0; i < vertices.Length; ++i) {
			line.SetPosition(i, vertices[i]);
		}
	}

	#endregion
}