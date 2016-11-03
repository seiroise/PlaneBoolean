using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 論理演算可能なオブジェクト
/// </summary>
public class BooleanObject : MonoBehaviour {

	public enum BooleanType {
		None,
		Or,
		Not
	}

	//ポリゴン
	public Polygon2 polygon;
	public BooleanType booleanType = BooleanType.None;

	//メッシュ関連
	private MeshFilter mf;
	private MeshRenderer mr;
	private PolygonCollider2D polyCo;

	//トリガーに引っ掛かったオブジェクト
	private HashSet<Collider2D> collectObjects;

	#region UnityEvent

	protected virtual void OnEnable() {
		
		//CollectObjects
		if (collectObjects == null) {
			collectObjects = new HashSet<Collider2D>();
		}
		//MeshFilter
		if(!mf) {
			mf = GetComponent<MeshFilter>();
		}
		//MeshRenderer
		if(!mr) {
			mr = GetComponent<MeshRenderer>();
		}
		//PolygonCollider
		if(!polyCo) {
			polyCo = GetComponent<PolygonCollider2D>();
		}
	}

	private void OnCollisionEnter2D(Collision2D co) {
		//自分がNotなら
		if (booleanType == BooleanType.Not) {
			polygon.Translate(transform.position);
			Not();
		}
	}

	private void OnTriggerEnter2D(Collider2D co) {
		//追加
		if (!collectObjects.Contains(co)) {
			collectObjects.Add(co);
		}
	}

	private void OnTriggerExit2D(Collider2D co) {
		//削除
		if (collectObjects.Contains(co)) {
			collectObjects.Remove(co);
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
		if(mf || polyCo) {
			Mesh mesh = polygon.ToMesh();
			if(mf) mf.mesh = mesh;
			if(polyCo) polyCo.SetPath(0, polygon.vertices);
		}
	}

	#endregion

	#region BooleanFunction

	/// <summary>
	/// 引っ掛かったオブジェクトに対して論理否定
	/// </summary>
	private void Not() {
		foreach (var e in collectObjects) {
			if (e == null) continue;
			BooleanObject boolObj = e.GetComponent<BooleanObject>();
			if (!boolObj) continue;
			boolObj.Not(polygon);
		}
		collectObjects.Clear();
	}

	/// <summary>
	/// 論理否定
	/// </summary>
	public void Not(Polygon2 poly) {

		polygon.Translate(transform.position);

		//分割
		List<List<Vector2>> divs = polygon.Not(poly);
		if (divs == null) return;
		for (int i = 0; i < divs.Count; ++i) {
			if (divs[i] == null || divs[i].Count == 0) continue;
			BooleanObject obj = Instantiate<BooleanObject>(this);
			obj.SetVertices(divs[i].ToArray());
		}

		Destroy(gameObject);
	}

	#endregion
}