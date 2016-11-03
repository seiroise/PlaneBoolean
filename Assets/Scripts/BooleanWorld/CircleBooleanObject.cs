using UnityEngine;
using System.Collections;

/// <summary>
/// 円形のブーリアンオブジェクト
/// </summary>
public class CircleBooleanObject : BooleanObject {

	[SerializeField, Range(0.01f, 100f)]
	private float radius = 1f;

	#region UnityEvent

	protected override void OnEnable() {
		base.OnEnable();

		//形状の設定
		int angle = 8;	//取り敢えず8角形

		Vector2[] vertices = new Vector2[angle];

		float angleDelta = 360f / angle;
		for (int i = 0; i < angle; ++i) {
			vertices[i] = GeomUtil.DegToVector2(angleDelta * i) * radius;
		}

		//頂点を設定
		SetVertices(vertices);
	}

	#endregion
}