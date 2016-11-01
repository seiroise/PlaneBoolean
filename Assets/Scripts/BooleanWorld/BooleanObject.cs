using UnityEngine;
using System.Collections;

/// <summary>
/// 論理演算可能なオブジェクト
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class BooleanObject : MonoBehaviour {

	//ポリゴン
	private Polygon2 polygon;

	//メッシュ関連
	private MeshFilter mf;
	private MeshRenderer mr;
	private MeshCollider mc;

	#region UnityEvent

	private void OnEnable() {
		//MeshFilter
		if(mf == null) {
			mf = GetComponent<MeshFilter>();
		}
		//MeshRenderer
		if(mr == null) {
			mr = GetComponent<MeshRenderer>();
		}
		//MeshCollider
		if(mc == null) {
			mc = GetComponent<MeshCollider>();
		}
	}

	#endregion

	#region Function

	/// <summary>
	/// 頂点の設定
	/// </summary>
	public void SetVertices(Vector2[] vertices) {

		//ポリゴンの作成
		polygon = new Polygon2(vertices);

		//メッシュの取得
		Mesh mesh = polygon.ToMesh();
		mf.mesh = mesh;
		mc.sharedMesh = mesh;
	}

	#endregion
}