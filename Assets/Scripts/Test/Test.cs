using UnityEngine;
using System.Linq;
using System.Collections;

public class Test : MonoBehaviour {

	private Polygon2 a;
	private Polygon2 b;

	public LineRenderer aLine;
	public LineRenderer bLine;

	#region UnityEvent

	private void Awake() {
		
		//テスト用頂点配列
		Vector2[] aVerts = new Vector2[] {
			new Vector2(0f, 0f),
			new Vector2(1f, 3f),
			new Vector2(4f, 3f),
			new Vector2(3f, 0f),
		};
		Vector2[] bVerts = new Vector2[] {
			new Vector2(4f, 2f),
			new Vector2(6f, 4f),
			new Vector2(8f, 2f),
			new Vector2(6f, 0f),
		};

		//線の描画
		SetLineRenderer(aLine, aVerts);
		SetLineRenderer(bLine, bVerts);

		//ポリゴンの生成
		a = new Polygon2(aVerts);
		b = new Polygon2(bVerts);

		//論理和のテスト
		a.Or(b);
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